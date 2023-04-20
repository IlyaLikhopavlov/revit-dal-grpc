using System;
using System.IO;
using System.Linq;
using System.Windows;
using App.CommunicationServices.Grpc;
using App.CommunicationServices.Revit;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories;
using App.DAL.Common.Repositories.DbRepositories;
using App.DAL.Common.Repositories.Factories;
using App.DAL.Common.Repositories.Factories.Base;
using App.DAL.Common.Repositories.RevitRepositories;
using App.DAL.Db;
using App.DAL.Db.Mapping;
using App.DAL.Db.Mapping.Abstractions;
using App.DAL.Revit.Converters;
using App.DAL.Revit.DataContext.RevitSets;
using App.DAL.Revit.DataContext;
using App.Services;
using App.Settings.Model;
using App.Settings.Model.Enums;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Bimdance.Framework.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using App.DAL.Db.Mapping.Profiles;
using App.DML;
using BarEntity = App.DAL.Db.Model.Bar;
using FooEntity = App.DAL.Db.Model.Foo;
using ProjectEntity = App.DAL.Db.Model.Project;
using CategoryEntity = App.DAL.Db.Model.Category;
using System.Collections.Generic;
using App.Catalog.Db;
using App.CommunicationServices.Utils.Comparers;
using App.DAL.Common.Services.Catalog;
using App.DAL.Revit.Converters.Common;
using App.Settings.Constants;

namespace AppUi.WebWindow
{
    public static class Bootstrap
    {
        public static string GetContentPath()
        {
            var result = Path.GetFullPath(
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", ".."));

            return result;
        }

        public static void Load(this IServiceCollection serviceCollection)
        {
            var contentPath = GetContentPath();
            
            
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = configurationBuilder.Build();

            serviceCollection.AddSingleton<MainWindow>();

            serviceCollection.AddWpfBlazorWebView();

            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddOptions();
            serviceCollection.Configure<ApplicationSettings>(options =>
                configuration.GetSection(nameof(ApplicationSettings)).Bind(options));
            serviceCollection.Configure<ConnectionStrings>(options =>
                configuration.GetSection(nameof(ConnectionStrings)).Bind(options));

            serviceCollection.AddSingleton<ApplicationObject>();
            serviceCollection.AddSingleton<IEqualityComparer<DocumentDescriptor>,
                DocumentDescriptorEqualityComparer>();
            serviceCollection.AddSingleton<ServiceScopeFactory<DocumentDescriptor>>();
            serviceCollection.AddSingleton<IDocumentDescriptorServiceScopeFactory,
                DocumentDescriptorServiceScopeFactory>();
            serviceCollection.AddSingleton<RevitActiveDocumentNotificationClient>();
            serviceCollection.AddSingleton<RevitExtraDataExchangeClient>();

            serviceCollection.AddScoped<RevitInstanceConverter<Bar>, BarConverter>();
            serviceCollection.AddScoped<RevitInstanceConverter<Foo>, FooConverter>();
            serviceCollection.AddScoped<BarSet>();
            serviceCollection.AddScoped<FooSet>();
            serviceCollection.AddScoped<IDataContext, DataContext>();
            
            serviceCollection.AddScoped<FooRevitRepository>();
            serviceCollection.AddScoped<BarRevitRepository>();
            serviceCollection.AddScoped<FooDbRepository>();
            serviceCollection.AddScoped<BarDbRepository>();
            serviceCollection.AddSingleton<IProjectRepository, ProjectDbRepository>();
            serviceCollection.AddSingleton<IRepositoryFactory<IFooRepository>, RepositoryFactory<IFooRepository>>();
            serviceCollection.AddSingleton<IRepositoryFactory<IBarRepository>, RepositoryFactory<IBarRepository>>();

            serviceCollection.AddScoped<ICatalogService, CatalogService>();
            serviceCollection.AddScoped<RevitCatalogStorage>();
            serviceCollection.AddScoped<DbCatalogStorage>();

            serviceCollection.AddSingleton<IEntityConverter<Project, ProjectEntity>, ProjectEntityConverter>();
            serviceCollection.AddSingleton<IEntityConverter<Foo, FooEntity>, FooEntityConverter>();
            serviceCollection.AddSingleton<IEntityConverter<Bar, BarEntity>, BarEntityConverter>();
            serviceCollection.AddSingleton<IEntityConverter<Category, CategoryEntity>, CategoryEntityConverter>();

            serviceCollection.AddSingleton<RevitDataService>();

            serviceCollection.AddTransient<ProjectsDbInitializer>();
            serviceCollection.AddTransient<CatalogDbInitializer>();
            serviceCollection.AddDbContextFactory<ProjectsDataContext>(builder =>
            {
                var connectionString = configuration
                                           .GetRequiredSection(nameof(ConnectionStrings))
                                           .Get<ConnectionStrings>()?.ProjectsDbConnection 
                                       ?? throw new InvalidOperationException(
                                           "Connection string for projects DB wasn't found.");
                builder.UseSqlite($"{DbConstants.SqLite.DataSourceParameterName}" +
                                  $"{Path.Combine(contentPath, connectionString)}");
            });
            serviceCollection.AddDbContextFactory<CatalogDbContext>(builder =>
            {
                var connectionString = configuration
                                           .GetRequiredSection(nameof(ConnectionStrings))
                                           .Get<ConnectionStrings>()?.CatalogDbConnection
                                       ?? throw new InvalidOperationException(
                                           "Connection string for catalog DB wasn't found.");
                builder.UseSqlite($"{DbConstants.SqLite.DataSourceParameterName}" +
                                  $"{Path.Combine(contentPath, connectionString)}");
            });

            serviceCollection.AddFactoryFacility();
            serviceCollection.AddAutoMapper(typeof(DbToDomainMappingProfile));
        }

        internal static void InitializeServices(this IServiceProvider serviceProvider)
        {
            try
            {
                serviceProvider.GetService<CatalogDbInitializer>()?.InitDataBase();

                var mode =
                    serviceProvider.GetService<IOptions<ApplicationSettings>>()?.Value.ApplicationMode;

                switch (mode)
                {
                    case null:
                        throw new InvalidOperationException("Configuration isn't available or incorrect.");
                    case ApplicationModeEnum.Desktop:
                        RunForRevit(serviceProvider);
                        break;
                    case ApplicationModeEnum.Web:
                    default:
                        RunForWeb(serviceProvider);
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.ToString(), 
                    "Error", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Error);
            }
        }

        private static void RunForRevit(IServiceProvider serviceProvider)
        {
            serviceProvider.GetService<RevitActiveDocumentNotificationClient>()?.RunGettingRevitNotification();
        }

        private static void RunForWeb(IServiceProvider serviceProvider)
        {
            serviceProvider.GetService<ProjectsDbInitializer>()?.InitDataBase();
            var applicationObject = serviceProvider.GetService<ApplicationObject>()
                ?? throw new ArgumentNullException(nameof(ApplicationObject));
            var projectRepository = serviceProvider.GetService<IProjectRepository>();

            var project = projectRepository?.GetAll().First();
            if (project is null)
            {
                throw new ArgumentNullException(nameof(project));
            }

            //stub for one document
            applicationObject.ActiveDocument = 
                new DocumentDescriptor
                {
                    Id = project.UniqueId,
                    Title = project.Title,
                    DocumentAction = DocumentActionEnum.Activated
                };
        }
    }
}

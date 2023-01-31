﻿using System;
using System.IO;
using System.Windows;
using App.CommunicationServices.Grpc;
using App.CommunicationServices.Revit;
using App.DAL.Common.Repositories;
using App.DAL.Common.Repositories.DbRepositories;
using App.DAL.Common.Repositories.Factories;
using App.DAL.Common.Repositories.RevitRepositories;
using App.DAL.Db;
using App.DAL.Revit.Converters;
using App.DAL.Revit.DataContext;
using App.DAL.Revit.DataContext.RevitSets;
using App.ScopedServicesFunctionality;
using App.Services;
using Bimdance.Framework.DependencyInjection;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AppUi.WebWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private void StartUp()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = configurationBuilder.Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();

            serviceCollection.AddSingleton<IConfiguration>(configuration);

            serviceCollection.AddSingleton<ApplicationObject>();
            serviceCollection.AddSingleton<IDocumentDescriptorServiceScopeFactory, DocumentDescriptorServiceScopeFactory>();
            serviceCollection.AddSingleton<RevitActiveDocumentNotificationClient>();
            serviceCollection.AddScoped<RevitExtraDataExchangeClient>();

            serviceCollection.AddScoped<BarConverter>();
            serviceCollection.AddScoped<FooConverter>();
            serviceCollection.AddScoped<BarSet>();
            serviceCollection.AddScoped<FooSet>();
            serviceCollection.AddScoped<IDataContext, DataContext>();
            serviceCollection.AddScoped<IFooRepository, FooDbRepository>();
            serviceCollection.AddScoped<IFooRepository, FooRevitRepository>();
            serviceCollection.AddScoped<IFooRepositoryFactory, FooRepositoryFactory>();

            serviceCollection.AddScoped<RevitDataService>();

            serviceCollection.AddTransient<ProjectsDbInitializer>();

            serviceCollection.AddDbContextFactory<ProjectsDataContext>(builder =>
            {
                //var path = Path.Combine(Environment.CurrentDirectory, "projects.db");
                builder.UseSqlite($"Data Source={configuration.GetConnectionString("DefaultConnection")}");
            });

            serviceCollection.AddFactoryFacility();

            Resources.Add("services", serviceCollection.BuildServiceProvider());
        }

        public MainWindow()
        {
            InitializeComponent();

            StartUp();

            if (Resources["services"] is not IServiceProvider serviceProvider)
            {
                return;
            }

            serviceProvider.GetService<RevitActiveDocumentNotificationClient>()?.RunGettingRevitNotification();
            serviceProvider.GetService<ProjectsDbInitializer>()?.InitDataBase();
        }
    }
}

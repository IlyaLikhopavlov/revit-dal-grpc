using System;
using System.IO;
using System.Windows;
using App.CommunicationServices.Grpc;
using App.CommunicationServices.Revit;
using App.DAL.Db;
using App.DAL.Revit.Converters;
using App.DAL.Revit.DataContext;
using App.DAL.Revit.DataContext.RevitSets;
using App.ScopedServicesFunctionality;
using App.Services;
using Bimdance.Framework.DependencyInjection;
using Microsoft.EntityFrameworkCore;
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
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddWpfBlazorWebView();

            serviceCollection.AddSingleton<ApplicationObject>();
            serviceCollection.AddSingleton<IDocumentDescriptorServiceScopeFactory, DocumentDescriptorServiceScopeFactory>();
            serviceCollection.AddSingleton<RevitActiveDocumentNotificationClient>();
            serviceCollection.AddScoped<RevitExtraDataExchangeClient>();

            serviceCollection.AddScoped<BarConverter>();
            serviceCollection.AddScoped<FooConverter>();
            serviceCollection.AddScoped<BarSet>();
            serviceCollection.AddScoped<FooSet>();
            serviceCollection.AddScoped<IDataContext, DataContext>();
            serviceCollection.AddScoped<RevitDataService>();

            serviceCollection.AddTransient<ProjectsDbInitializer>();

            serviceCollection.AddDbContextFactory<ProjectsDataContext>();

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

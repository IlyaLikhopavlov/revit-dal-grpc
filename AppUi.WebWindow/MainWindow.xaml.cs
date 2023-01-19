using System;
using System.Windows;
using App.CommunicationServices.Grpc;
using App.CommunicationServices.Revit;
using App.DAL.Converters;
using App.DAL.DataContext.RevitSets;
using App.DAL.DataContext;
using App.ScopedServicesFunctionality;
using App.Services;
using Bimdance.Framework.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Revit.Services.Grpc.Services;

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

            serviceCollection.AddSingleton<RevitApplication>();
            serviceCollection.AddSingleton<IDocumentDescriptorServiceScopeFactory, DocumentDescriptorServiceScopeFactory>();
            serviceCollection.AddSingleton<RevitActiveDocumentNotificationClient>();
            serviceCollection.AddScoped<RevitExtraDataExchangeClient>();

            serviceCollection.AddScoped<BarConverter>();
            serviceCollection.AddScoped<FooConverter>();
            serviceCollection.AddScoped<BarSet>();
            serviceCollection.AddScoped<FooSet>();
            serviceCollection.AddScoped<IDataContext, DataContext>();
            serviceCollection.AddScoped<RevitDataService>();

            serviceCollection.AddFactoryFacility();

            Resources.Add("services", serviceCollection.BuildServiceProvider());
        }

        public MainWindow()
        {
            InitializeComponent();

            StartUp();

            if (Resources["services"] is IServiceProvider serviceProvider)
            {
                serviceProvider.GetService<RevitActiveDocumentNotificationClient>()?.RunGettingRevitNotification();
            }
        }
    }
}

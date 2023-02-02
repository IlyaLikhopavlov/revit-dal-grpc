using System;
using System.IO;
using System.Windows;
using App.CommunicationServices.Grpc;
using App.CommunicationServices.Revit;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories;
using App.DAL.Common.Repositories.DbRepositories;
using App.DAL.Common.Repositories.Factories;
using App.DAL.Common.Repositories.RevitRepositories;
using App.DAL.Db;
using App.DAL.Revit.Converters;
using App.DAL.Revit.DataContext;
using App.DAL.Revit.DataContext.RevitSets;
using App.Services;
using App.Settings.Model;
using App.Settings.Model.Enums;
using Bimdance.Framework.DependencyInjection;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AppUi.WebWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var serviceCollection = new ServiceCollection();
            serviceCollection.Load();
            
            Resources.Add("services", serviceCollection.BuildServiceProvider());

            if (Resources["services"] is not IServiceProvider serviceProvider)
            {
                MessageBox.Show("Service provider didn't find.");
                return;
            }

            serviceProvider.InitializeServices();
        }
    }
}

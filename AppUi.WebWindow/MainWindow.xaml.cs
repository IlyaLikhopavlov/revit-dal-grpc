using System;
using System.Windows;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Extensions.DependencyInjection;

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
            //BlazorWebView.Services = App.AppHost.Services;


            //var serviceCollection = new ServiceCollection();
            //serviceCollection.Load();

            //Resources.Add("services", serviceCollection.BuildServiceProvider());

            //if (Resources["services"] is not IServiceProvider serviceProvider)
            //{
            //    MessageBox.Show("Service provider didn't find.");
            //    return;
            //}

            //serviceProvider.InitializeServices();
        }
    }
}

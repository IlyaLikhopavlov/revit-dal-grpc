using System;
using System.Windows;
using Microsoft.AspNetCore.Components.WebView;
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
        }

        private void Handle_UrlLoading(object sender,
            UrlLoadingEventArgs urlLoadingEventArgs)
        {
            if (urlLoadingEventArgs.Url.Host != "0.0.0.0")
            {
                urlLoadingEventArgs.UrlLoadingStrategy =
                    UrlLoadingStrategy.OpenInWebView;
            }
        }
    }
}

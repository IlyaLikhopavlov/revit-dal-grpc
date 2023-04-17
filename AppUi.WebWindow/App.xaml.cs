using System;
using System.IO;
using System.Windows;
using AppUi.WebWindow.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppUi.WebWindow
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            RevitRunner.DisableBimCd();
            
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.Load();
                })
                .Build();

            Resources.Add("services", AppHost.Services);

            AppHost.Services.InitializeServices();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            
            RevitRunner.EnableBimCd();

            base.OnExit(e);
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            await AppHost.StartAsync();

            var startupForm = AppHost.Services.GetRequiredService<MainWindow>();
            startupForm.Show();
            
            base.OnStartup(e);
        }
    }
}

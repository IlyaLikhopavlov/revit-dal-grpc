using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AppUi.WebWindow
{
    public partial class App : Application
    {
        public static IHost AppHost { get; private set; }

        public App()
        {
            AppHost = Host.CreateDefaultBuilder()
                .ConfigureServices((hostContext, services) =>
                {
                    services.Load();
                })
                .Build();

            Resources.Add("services", AppHost.Services);

            if (Resources["services"] is not IServiceProvider serviceProvider)
            {
                MessageBox.Show("Service provider didn't find.");
                return;
            }

            serviceProvider.InitializeServices();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            await AppHost.StopAsync();
            
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.ScopedServicesFunctionality;
using App.Services.Revit;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;
using Revit.Services.Grpc.Services;

namespace App.Main
{
    internal class StartUp
    {

        public static IServiceProvider ServiceProvider { get; private set; }
        
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<RevitApplication>();
            serviceCollection.AddSingleton<IDocumentDescriptorServiceScopeFactory, DocumentDescriptorServiceScopeFactory>();
            serviceCollection.AddSingleton<Services.Grpc.RevitActiveDocumentNotificationClient>();
            serviceCollection.AddSingleton<Services.Grpc.RevitExtraDataExchangeClient>();
        }

        public void Build()
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        public void ShutDown()
        {
            
        }
    }
}

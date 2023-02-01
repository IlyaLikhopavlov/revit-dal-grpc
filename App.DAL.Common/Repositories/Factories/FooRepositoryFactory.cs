using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.CommunicationServices.Revit;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories.DbRepositories;
using App.DAL.Common.Repositories.RevitRepositories;
using Microsoft.Extensions.Configuration;

namespace App.DAL.Common.Repositories.Factories
{
    public class FooRepositoryFactory : IFooRepositoryFactory
    {
        private readonly IConfiguration _configuration;

        private readonly IDocumentDescriptorServiceScopeFactory _documentDescriptorServiceScopeFactory;

        public FooRepositoryFactory(
            IConfiguration configuration, 
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory)
        {
            _configuration = configuration;
            _documentDescriptorServiceScopeFactory = documentDescriptorServiceScopeFactory;
        }

        public IFooRepository Create()
        {
            return 
                _configuration["ApplicationSettings:Mode"] switch
                {
                    "web" => _documentDescriptorServiceScopeFactory.GetScopedService<FooDbRepository>(),
                    "desktop" => _documentDescriptorServiceScopeFactory.GetScopedService<FooRevitRepository>(),
                    _ => throw new InvalidEnumArgumentException("Required repository type didn't find")
                };
        }
    }
}

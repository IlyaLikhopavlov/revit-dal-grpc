using App.DAL.Common.Repositories.Factories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.Settings.Model;
using Microsoft.Extensions.Options;
using App.DAL.Common.Repositories.RevitRepositories;
using App.DAL.Common.Repositories.DbRepositories;

namespace App.DAL.Common.Repositories.Factories
{
    public class BarRepositoryFactory : RepositoryFactoryBase<IBarRepository>
    {
        public BarRepositoryFactory(
            IOptions<ApplicationSettings> options, 
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory) 
            : base(options, documentDescriptorServiceScopeFactory)
        {
        }

        protected override IBarRepository CreateRevitRepository()
        {
            return (BarRevitRepository)DocumentDescriptorServiceScopeFactory
                .GetScopedService(typeof(BarRevitRepository));
        }

        protected override IBarRepository CreateDbRepository()
        {
            return (BarDbRepository)DocumentDescriptorServiceScopeFactory
                .GetScopedService(typeof(BarDbRepository));
        }
    }
}

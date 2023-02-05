using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories.DbRepositories;
using App.DAL.Common.Repositories.Factories.Base;
using App.DAL.Common.Repositories.RevitRepositories;
using App.Settings.Model;
using Microsoft.Extensions.Options;

namespace App.DAL.Common.Repositories.Factories
{
    public class FooRepositoryFactory : RepositoryFactoryBase<IFooRepository>
    {

        public FooRepositoryFactory(
            IOptions<ApplicationSettings> options, 
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory) 
            : base(options, documentDescriptorServiceScopeFactory)
        {
        }

        protected override IFooRepository CreateRevitRepository()
        {
            return (FooRevitRepository)DocumentDescriptorServiceScopeFactory
                .GetScopedService(typeof(FooRevitRepository));
        }

        protected override IFooRepository CreateDbRepository()
        {
            return (FooDbRepository)DocumentDescriptorServiceScopeFactory
                .GetScopedService(typeof(FooDbRepository));
        }
    }
}

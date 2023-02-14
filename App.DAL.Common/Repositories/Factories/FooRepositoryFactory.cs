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
            return CreateRepository<FooRevitRepository>();
        }

        protected override IFooRepository CreateDbRepository()
        {
            return CreateRepository<FooDbRepository>();
        }
    }
}

using System.ComponentModel;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Repositories.DbRepositories;
using App.DAL.Common.Repositories.RevitRepositories;
using App.Settings.Model;
using App.Settings.Model.Enums;
using Microsoft.Extensions.Options;

namespace App.DAL.Common.Repositories.Factories
{
    public class FooRepositoryFactory : IFooRepositoryFactory
    {
        private readonly IOptions<ApplicationSettings> _options;

        private readonly IDocumentDescriptorServiceScopeFactory _documentDescriptorServiceScopeFactory;

        public FooRepositoryFactory(
            IOptions<ApplicationSettings> options, 
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory)
        {
            _options = options;
            _documentDescriptorServiceScopeFactory = documentDescriptorServiceScopeFactory;
        }

        public IFooRepository Create()
        {
            return 
                _options.Value.ApplicationMode switch
                {
                    ApplicationModeEnum.Web => 
                        _documentDescriptorServiceScopeFactory.GetScopedService<FooDbRepository>(),

                    ApplicationModeEnum.Desktop => 
                        _documentDescriptorServiceScopeFactory.GetScopedService<FooRevitRepository>(),

                    _ => throw new InvalidEnumArgumentException("Required repository type didn't find")
                };
        }
    }
}

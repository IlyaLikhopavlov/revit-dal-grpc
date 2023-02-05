using App.Settings.Model;
using Microsoft.Extensions.Options;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.Settings.Model.Enums;
using System.ComponentModel;

namespace App.DAL.Common.Repositories.Factories.Base
{
    public abstract class RepositoryFactoryBase<T> : IRepositoryFactory<T> where T : class
    {
        private readonly IOptions<ApplicationSettings> _options;

        protected readonly IDocumentDescriptorServiceScopeFactory DocumentDescriptorServiceScopeFactory;

        protected RepositoryFactoryBase(
            IOptions<ApplicationSettings> options,
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory)
        {
            _options = options;
            DocumentDescriptorServiceScopeFactory = documentDescriptorServiceScopeFactory;
        }

        protected abstract T CreateRevitRepository();

        protected abstract T CreateDbRepository();

        public T Create()
        {
            return
                _options.Value.ApplicationMode switch
                {
                    ApplicationModeEnum.Web => CreateDbRepository(),
                    ApplicationModeEnum.Desktop => CreateRevitRepository(),
                    _ => throw new InvalidEnumArgumentException("Required repository type didn't find")
                };
        }
    }
}

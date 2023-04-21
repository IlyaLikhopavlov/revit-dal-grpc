using App.Settings.Model;
using Microsoft.Extensions.Options;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.Settings.Model.Enums;
using System.ComponentModel;
using App.DAL.Common.Repositories.DbRepositories;
using App.DAL.Common.Repositories.RevitRepositories;
using App.DAL.Db;

namespace App.DAL.Common.Repositories.Factories.Base
{
    public class RepositoryFactory<T> : IRepositoryFactory<T> where T : class
    {
        private readonly IOptions<ApplicationSettings> _options;

        protected readonly IDocumentDescriptorServiceScopeFactory DocumentDescriptorServiceScopeFactory;

        private readonly Dictionary<Type, (Type Db, Type RevitStorage)> _repositoriesDictionary =
            new()
            {
                { typeof(IFooRepository), (typeof(FooDbRepository), typeof(FooRevitRepository)) },
                { typeof(IBarRepository), (typeof(BarDbRepository), typeof(BarRevitRepository)) },
            };

        public RepositoryFactory(
            IOptions<ApplicationSettings> options,
            IDocumentDescriptorServiceScopeFactory documentDescriptorServiceScopeFactory)
        {
            _options = options;
            DocumentDescriptorServiceScopeFactory = documentDescriptorServiceScopeFactory;
        }
        
        public T Create(ProjectsDataContext projectsDataContext)
        {
            if (!_repositoriesDictionary.TryGetValue(typeof(T), out var repositories))
            {
                throw new InvalidOperationException(typeof(T).FullName);
            }

            return
                _options.Value.ApplicationMode switch
                {
                    ApplicationModeEnum.Web => DocumentDescriptorServiceScopeFactory.GetScopedService(repositories.Db) as T,
                    ApplicationModeEnum.Desktop => DocumentDescriptorServiceScopeFactory.GetScopedService(repositories.RevitStorage) as T,
                    _ => throw new InvalidEnumArgumentException("Required repository type didn't find")
                };
        }
    }
}

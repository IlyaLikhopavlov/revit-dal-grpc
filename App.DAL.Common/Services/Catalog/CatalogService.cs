using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Catalog.Db.Model;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.Settings.Model;
using App.Settings.Model.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace App.DAL.Common.Services.Catalog
{
    public class CatalogService
    {
        private readonly ICatalogStorage _catalogStorage; 
        
        public CatalogService(
            IOptions<ApplicationSettings> options, 
            IDocumentDescriptorServiceScopeFactory serviceScopeFactory)
        {
            _catalogStorage = options.Value.ApplicationMode switch
            {
                ApplicationModeEnum.Web => serviceScopeFactory.GetScopedService<DbCatalogStorage>(),
                ApplicationModeEnum.Desktop => serviceScopeFactory.GetScopedService<RevitCatalogStorage>(),
                _ => throw new InvalidEnumArgumentException("Required repository type didn't find")
            };
        }

        public async Task<T> ReadCatalogRecord<T>(Guid uniqueId) where T : BaseCatalogEntity
        {
            return await _catalogStorage.ReadCatalogRecord<T>(uniqueId);
        }

        public async Task WriteCatalogRecord<T>(T record) where T : BaseCatalogEntity
        {
            await _catalogStorage.WriteCatalogRecord<T>(record);
        }
    }
}

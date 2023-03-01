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
        private ICatalogStorage _revitCatalogStorage;

        private ICatalogStorage _dbCatalogStorage;

        private readonly ApplicationModeEnum _mode;
        
        public CatalogService(
            IOptions<ApplicationSettings> options, 
            IDocumentDescriptorServiceScopeFactory serviceScopeFactory)
        {
            _mode = options.Value.ApplicationMode;

            switch (_mode)
            {
                case ApplicationModeEnum.Desktop:
                    GetStoragesForDesktop(serviceScopeFactory);
                    break;
                case ApplicationModeEnum.Web:
                    GetStorageForWeb(serviceScopeFactory);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void GetStorageForWeb(IDocumentDescriptorServiceScopeFactory factory)
        {
            _dbCatalogStorage = factory.GetScopedService<DbCatalogStorage>();
        }

        public void GetStoragesForDesktop(IDocumentDescriptorServiceScopeFactory factory)
        {
            _dbCatalogStorage = factory.GetScopedService<DbCatalogStorage>();
            _revitCatalogStorage = factory.GetScopedService<RevitCatalogStorage>();
        }

        public async Task<T> ReadCatalogRecord<T>(Guid uniqueId) where T : BaseCatalogEntity
        {
            switch (_mode)
            {
                case ApplicationModeEnum.Web:
                    return await _dbCatalogStorage.ReadCatalogRecord<T>(uniqueId);
                case ApplicationModeEnum.Desktop:
                {
                    var recordFromDocument = _revitCatalogStorage.ReadCatalogRecord<T>(uniqueId);

                    //TODO check result, if record didn't find, try to get it from DB
                    break;
                }
            }
        }

        public async Task WriteCatalogRecord<T>(T record) where T : BaseCatalogEntity
        {
            //TODO
        }
    }
}

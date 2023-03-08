using App.Catalog.Db.Model;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.Settings.Model;
using App.Settings.Model.Enums;
using Microsoft.Extensions.Options;

namespace App.DAL.Common.Services.Catalog
{
    public class CatalogService : ICatalogService
    {
        private readonly IDocumentDescriptorServiceScopeFactory _serviceScopeFactory;
        private readonly ApplicationModeEnum _mode;
        
        public CatalogService(
            IOptions<ApplicationSettings> options, 
            IDocumentDescriptorServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mode = options.Value.ApplicationMode;
        }

        public async Task<T> ReadCatalogRecordAsync<T>(Guid uniqueId) where T : BaseCatalogEntity
        {
            switch (_mode)
            {
                case ApplicationModeEnum.Web:
                    var dbCatalogStorage = _serviceScopeFactory.GetScopedService<DbCatalogStorage>();
                    return await dbCatalogStorage.ReadCatalogRecordOrDefaultAsync<T>(uniqueId);
                case ApplicationModeEnum.Desktop:
                {
                    var revitCatalogStorage = _serviceScopeFactory.GetScopedService<RevitCatalogStorage>();
                    var record = await revitCatalogStorage.ReadCatalogRecordOrDefaultAsync<T>(uniqueId);
                
                    if (record is not null)
                    {
                        return record;
                    }
                    
                    dbCatalogStorage = _serviceScopeFactory.GetScopedService<DbCatalogStorage>();
                    record = await dbCatalogStorage.ReadCatalogRecordOrDefaultAsync<T>(uniqueId);

                    await revitCatalogStorage.WriteCatalogRecordAsync(record);

                    return record;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task WriteCatalogRecordAsync<T>(T t) where T : BaseCatalogEntity
        {
            switch (_mode)
            {
                case ApplicationModeEnum.Web:
                {
                    var dbCatalogStorage = _serviceScopeFactory.GetScopedService<DbCatalogStorage>();
                    await dbCatalogStorage.WriteCatalogRecordAsync(t);
                    break;
                }
                case ApplicationModeEnum.Desktop:
                {
                    var dbCatalogStorage = _serviceScopeFactory.GetScopedService<DbCatalogStorage>();
                    var revitCatalogStorage = _serviceScopeFactory.GetScopedService<RevitCatalogStorage>();
                    
                    await dbCatalogStorage.WriteCatalogRecordAsync(t);
                    await revitCatalogStorage.WriteCatalogRecordAsync(t);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}

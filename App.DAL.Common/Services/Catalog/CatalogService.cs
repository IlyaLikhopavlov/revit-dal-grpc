using App.Catalog.Db.Model;
using App.CommunicationServices.ScopedServicesFunctionality;
using App.DAL.Common.Services.Catalog.Model;
using App.DAL.Common.Services.Catalog.Model.Enums;
using App.Settings.Model;
using App.Settings.Model.Enums;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Options;

namespace App.DAL.Common.Services.Catalog
{
    public class CatalogService : ICatalogService
    {
        private readonly IDocumentDescriptorServiceScopeFactory _serviceScopeFactory;
        private readonly ApplicationModeEnum _mode;
        private readonly DocumentDescriptor _documentDescriptor;
        
        public CatalogService(
            IOptions<ApplicationSettings> options, 
            IDocumentDescriptorServiceScopeFactory serviceScopeFactory,
            DocumentDescriptor documentDescriptor)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _mode = options.Value.ApplicationMode;
            _documentDescriptor = documentDescriptor;
        }

        public async Task<T> ReadCatalogRecordAsync<T>(
            Guid uniqueId,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T : BaseCatalogEntity
        {
            switch (_mode)
            {
                case ApplicationModeEnum.Web:
                    var dbCatalogStorage = _serviceScopeFactory.GetScopedService<DbCatalogStorage>();
                    return dbCatalogStorage.ReadCatalogRecordOrDefault(uniqueId, include);
                case ApplicationModeEnum.Desktop:
                {
                    var revitCatalogStorage = _serviceScopeFactory.GetScopedService<RevitCatalogStorage>();
                    var record = await revitCatalogStorage.ReadCatalogRecordOrDefaultAsync<T>(uniqueId);
                
                    if (record is not null)
                    {
                        return record;
                    }
                    
                    dbCatalogStorage = _serviceScopeFactory.GetScopedService<DbCatalogStorage>();
                    record = dbCatalogStorage.ReadCatalogRecordOrDefault(uniqueId, include);

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
                    dbCatalogStorage.WriteCatalogRecord(t);
                    break;
                }
                case ApplicationModeEnum.Desktop:
                {
                    var dbCatalogStorage = _serviceScopeFactory.GetScopedService<DbCatalogStorage>();
                    var revitCatalogStorage = _serviceScopeFactory.GetScopedService<RevitCatalogStorage>();
                    
                    dbCatalogStorage.WriteCatalogRecord(t);
                    await revitCatalogStorage.WriteCatalogRecordAsync(t);
                    break;
                }
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public async Task<IEnumerable<CatalogRecordComparisonResult>> CompareAllAsync()
        {
            if (_mode == ApplicationModeEnum.Web)
            {
                throw new InvalidOperationException("Inappropriate mode");
            }

            var dbCatalogStorage = _serviceScopeFactory.GetScopedService<DbCatalogStorage>();
            var revitCatalogStorage = _serviceScopeFactory.GetScopedService<RevitCatalogStorage>();

            var results = 
                (await revitCatalogStorage.ReadAllCatalogRecordsAsync())
                    .Select(r =>
                    {
                        var dbCatalogRecord =
                            dbCatalogStorage
                                .ReadAllCatalogRecords()
                                .FirstOrDefault(b => r.IdGuid == b.IdGuid);

                        var dbRecordType = dbCatalogRecord?.GetType() ?? r.GetType();

                        return
                            new CatalogRecordComparisonResult
                            {
                                IdGuid = r.IdGuid,
                                DbVersion = dbCatalogRecord?.Version ?? 0,
                                DocumentVersion = r.Version,
                                ModelNumber = r.ModelNumber == dbCatalogRecord?.ModelNumber
                                    ? r.ModelNumber
                                    : string.Empty,
                                PartNumber = r.PartNumber == dbCatalogRecord?.PartNumber
                                    ? r.PartNumber
                                    : string.Empty,
                                Type = dbRecordType
                            };
                    });

            return results;
        }

        public async Task SynchronizeAsync(IEnumerable<CatalogRecordComparisonResult> comparisonResults)
        {
            if (comparisonResults is null)
            {
                return;
            }

            var requiredResults = comparisonResults
                .Where(x => !x.IsIgnored 
                            && x.Resolution != ResolutionEnum.NothingToDo)
                .ToList();

            if (!requiredResults.Any())
            {
                return;
            }

            var revitCatalogStorage = _serviceScopeFactory.GetScopedService<RevitCatalogStorage>();
            var dbCatalogStorage = _serviceScopeFactory.GetScopedService<DbCatalogStorage>();

            foreach (var result in requiredResults)
            {
                if (result.Resolution == ResolutionEnum.UpdateInDocument)
                {
                    var record = 
                        dbCatalogStorage.ReadCatalogRecordOrDefault(result.IdGuid, result.Type);
                    await revitCatalogStorage.WriteCatalogRecordAsync(record, result.Type);
                }
                else
                {
                    var record = 
                        await revitCatalogStorage.ReadCatalogRecordOrDefaultAsync(result.IdGuid, result.Type);
                    dbCatalogStorage.WriteCatalogRecord(record);
                }
            }
        } 
    }
}

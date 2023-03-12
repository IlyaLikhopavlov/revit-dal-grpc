using App.Catalog.Db.Model;
using App.DAL.Common.Services.Catalog.Model;

namespace App.DAL.Common.Services.Catalog;

public interface ICatalogService
{
    Task<T> ReadCatalogRecordAsync<T>(Guid uniqueId) where T : BaseCatalogEntity;
    Task WriteCatalogRecordAsync<T>(T t) where T : BaseCatalogEntity;
    Task<IEnumerable<CatalogRecordComparisonResult>> CompareAll();
}
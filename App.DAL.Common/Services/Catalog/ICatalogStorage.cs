using App.Catalog.Db.Model;

namespace App.DAL.Common.Services.Catalog
{
    public interface ICatalogStorage
    {
        Task<T> ReadCatalogRecordOrDefaultAsync<T>(Guid uniqueId) where T : BaseCatalogEntity;

        Task WriteCatalogRecordAsync<T>(T record) where T : BaseCatalogEntity;
    }
}

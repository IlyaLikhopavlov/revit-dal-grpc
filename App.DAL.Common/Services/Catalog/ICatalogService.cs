using App.Catalog.Db.Model;
using App.DAL.Common.Services.Catalog.Model;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace App.DAL.Common.Services.Catalog;

public interface ICatalogService
{
    Task<T> ReadCatalogRecordAsync<T>(Guid uniqueId,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null) where T : BaseCatalogEntity;
    Task WriteCatalogRecordAsync<T>(T t) where T : BaseCatalogEntity;
    Task<IEnumerable<CatalogRecordComparisonResult>> CompareAllAsync();
    Task SynchronizeAsync(IEnumerable<CatalogRecordComparisonResult> comparisonResults);
}
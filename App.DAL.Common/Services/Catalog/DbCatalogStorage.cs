using App.Catalog.Db;
using App.Catalog.Db.Model;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Common.Services.Catalog
{
    public class DbCatalogStorage : ICatalogStorage
    {
        protected readonly CatalogDbContext DbContext;

        private readonly DocumentDescriptor _documentDescriptor;

        public DbCatalogStorage(
            IDbContextFactory<CatalogDbContext> dbContextFactory, 
            DocumentDescriptor documentDescriptor)
        {
            DbContext = dbContextFactory.CreateDbContext();
            _documentDescriptor = documentDescriptor;
        }

        public async Task<T> ReadCatalogRecordOrDefaultAsync<T>(Guid uniqueId) where T : BaseCatalogEntity
        {
            var set = DbContext.Set<T>();
            var entity = await set.FirstOrDefaultAsync(x => x.IdGuid == uniqueId);
            return entity;
        }

        public async Task WriteCatalogRecordAsync<T>(T record) where T : BaseCatalogEntity
        {
            var set = DbContext.Set<T>();
            var entity = set.FirstOrDefault(x => x.IdGuid == record.IdGuid);

            if (entity is null)
            {
                set.Add(record);
            }
            else
            {
                record.Id = entity.Id;
                set.Update(record);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}

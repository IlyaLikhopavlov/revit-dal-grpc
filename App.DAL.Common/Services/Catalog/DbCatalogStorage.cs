using System.Security.Cryptography.X509Certificates;
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
            var entity = await set.AsNoTracking().FirstOrDefaultAsync(x => x.IdGuid == uniqueId);
            return entity;
        }

        public async Task WriteCatalogRecordAsync<T>(T record) where T : BaseCatalogEntity
        {
            var set = DbContext.Set<T>();
            var existingEntity = set.AsNoTracking().FirstOrDefault(x => x.IdGuid == record.IdGuid);
            DbContext.ChangeTracker.Clear();

            if (existingEntity is null)
            {
                record.Version = 1;
                set.Add(record);
            }
            else
            {
                var currentVersion = existingEntity.Version;

                if (currentVersion >= long.MaxValue)
                {
                    record.Version = 1;
                }
                else
                {
                    record.Version = ++currentVersion;
                }

                record.Id = existingEntity.Id;
                set.Update(record);
            }

            await DbContext.SaveChangesAsync();
        }
    }
}

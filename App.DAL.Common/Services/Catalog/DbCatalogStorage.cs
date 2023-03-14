using System.Security.Cryptography.X509Certificates;
using App.Catalog.Db;
using App.Catalog.Db.Model;
using App.Catalog.Db.Tools;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace App.DAL.Common.Services.Catalog
{
    public class DbCatalogStorage
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

        public T ReadCatalogRecordOrDefault<T>(Guid uniqueId) where T : BaseCatalogEntity
        {
            var set = DbContext.Set<T>();
            var entity = set.AsNoTracking().FirstOrDefault(x => x.IdGuid == uniqueId);
            return entity;
        }

        public BaseCatalogEntity ReadCatalogRecordOrDefault(Guid uniqueId, Type recordType)
        {
            return DbContext
                .GetEntityQuery(recordType)
                .AsNoTracking()
                .FirstOrDefault(x => x.IdGuid == uniqueId);
        }

        public void WriteCatalogRecord(BaseCatalogEntity record)
        {
            var method = typeof(DbCatalogStorage).GetMethods()
                .Single(p =>
                    p.Name == nameof(WriteCatalogRecord) &&
                    p.ContainsGenericParameters);

            var genericMethod = method.MakeGenericMethod(record.GetType());
            genericMethod.Invoke(record, new object[] { record.IdGuid });
        }

        public void WriteCatalogRecord<T>(T record) where T : BaseCatalogEntity
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

            DbContext.SaveChanges();
        }

        public IEnumerable<BaseCatalogEntity> ReadAllCatalogRecords()
        {
            var dbEntities = DbContext
                .GetAllEntityQueries()
                .SelectMany(x => x);

            return dbEntities;
        }
    }
}

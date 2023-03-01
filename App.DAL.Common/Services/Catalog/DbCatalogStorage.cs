using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Catalog.Db;
using App.Catalog.Db.Model;
using App.DAL.Db.Mapping.Abstractions;
using App.DAL.Db;
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

        public Task<T> ReadCatalogRecord<T>(Guid uniqueId) where T : BaseCatalogEntity
        {
            var set = DbContext.Set<T>();
            var entity = set.First(x => x.IdGuid == uniqueId);
            return Task.FromResult(entity);
        }

        public Task WriteCatalogRecord<T>(T record) where T : BaseCatalogEntity
        {
            var set = DbContext.Set<T>();
            var entity = set.FirstOrDefault(x => x.IdGuid == record.IdGuid);

            if (entity is null)
            {
                set.Add(record);
            }
            else
            {
                //TODO Update
            }

            DbContext.SaveChanges();
            return Task.CompletedTask;
        }
    }
}

using App.Catalog.Db.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DAL.Common.Services.Catalog
{
    public interface ICatalogStorage
    {
        Task<T> ReadCatalogRecord<T>(Guid uniqueId) where T : BaseCatalogEntity;

        Task WriteCatalogRecord<T>(T record) where T : BaseCatalogEntity;
    }
}

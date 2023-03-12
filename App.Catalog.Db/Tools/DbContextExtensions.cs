using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using App.Catalog.Db.Model;

namespace App.Catalog.Db.Tools
{
    public static class DbContextExtensions
    {
        public static IQueryable<BaseCatalogEntity> GetEntityQuery(this DbContext context, Type entityType)
        {
            var method = typeof(DbContext).GetMethods()
                .Single(p =>
                    p.Name == nameof(DbContext.Set) && 
                    p.ContainsGenericParameters && 
                    !p.GetParameters().Any());

            var genericMethod = method.MakeGenericMethod(entityType);
            var dbSet = genericMethod.Invoke(context, null);
            return (IQueryable<BaseCatalogEntity>)dbSet;
        }

        public static IEnumerable<IQueryable<BaseCatalogEntity>> GetAllEntityQueries(this DbContext context)
        {
            return context.Model
                .GetEntityTypes()
                .Where(x => x.ClrType.BaseType == typeof(BaseCatalogEntity))
                .Select(entityType => context.GetEntityQuery(entityType.ClrType));
        }
    }
}

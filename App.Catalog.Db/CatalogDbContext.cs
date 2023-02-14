using App.Catalog.Db.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace App.Catalog.Db
{
    public class CatalogDbContext : DbContext
    {
        protected CatalogDbContext()
        {
        }

        public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
        {
        }

        public DbSet<FooCatalog> FooCatalog { get; set; }

        public DbSet<BarCatalog> BarCatalog { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlite("Data Source=catalog.db");
        }
    }
}

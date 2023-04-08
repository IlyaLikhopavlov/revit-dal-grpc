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

        public DbSet<Channel> Channel { get; set; }

        public DbSet<PowerType> PowerTypes { get; set; }

        public DbSet<BarCatalogChannel> BarCatalogChannel { get; set; }

        public DbSet<FooCatalogChannel> FooCatalogChannel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FooCatalogChannel>()
                .HasKey(fc => new { fc.FooCatalogId, fc.ChannelId });

            modelBuilder.Entity<BarCatalogChannel>()
                .HasKey(bc => new { bc.BarCatalogId, bc.ChannelId });

            modelBuilder.Entity<FooCatalogChannel>()
                .HasOne(fc => fc.FooCatalog)
                .WithMany(f => f.FooCatalogChannels)
                .HasForeignKey(fc => fc.FooCatalogId);

            modelBuilder.Entity<FooCatalogChannel>()
                .HasOne(fc => fc.Channel)
                .WithMany(f => f.FooCatalogChannels)
                .HasForeignKey(fc => fc.ChannelId);

            modelBuilder.Entity<BarCatalogChannel>()
                .HasOne(bc => bc.BarCatalog)
                .WithMany(f => f.BarCatalogChannels)
                .HasForeignKey(bc => bc.BarCatalogId);

            modelBuilder.Entity<BarCatalogChannel>()
                .HasOne(bc => bc.Channel)
                .WithMany(b => b.BarCatalogChannels)
                .HasForeignKey(bc => bc.ChannelId);

            modelBuilder.Entity<PowerType>()
                .HasMany(p => p.FooCatalogs)
                .WithOne(f => f.PowerType)
                .HasForeignKey(f => f.PowerTypeId);

            modelBuilder.Entity<PowerType>()
                .HasMany(p => p.BarCatalogs)
                .WithOne(f => f.PowerType)
                .HasForeignKey(f => f.PowerTypeId);
        }
    }
}

using App.Settings.Model;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using App.Settings.Constants;

namespace App.Catalog.Db.Tools
{
    internal class CatalogDbContextDesignTimeFactory : IDesignTimeDbContextFactory<CatalogDbContext>
    {
        public CatalogDbContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = configurationBuilder.Build();

            var connectionString = configuration
                                       .GetRequiredSection(nameof(ConnectionStrings))
                                       .Get<ConnectionStrings>()?.CatalogDbConnection ?? throw new InvalidOperationException(
                                       "Connection string for catalog DB wasn't found.");

            var optionsBuilder = new DbContextOptionsBuilder<CatalogDbContext>();
            optionsBuilder.UseSqlite($"{DbConstants.SqLite.DataSourceParameterName}{connectionString}");

            return new CatalogDbContext(optionsBuilder.Options);
        }
    }
}

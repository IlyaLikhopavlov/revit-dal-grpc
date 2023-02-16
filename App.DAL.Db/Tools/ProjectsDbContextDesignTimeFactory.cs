using App.Settings.Constants;
using App.Settings.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace App.DAL.Db.Tools
{
    internal class ProjectsDbContextDesignTimeFactory : IDesignTimeDbContextFactory<ProjectsDataContext>
    {
        public ProjectsDataContext CreateDbContext(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = configurationBuilder.Build();

            var connectionString = configuration
                .GetRequiredSection(nameof(ConnectionStrings))
                .Get<ConnectionStrings>()?.ProjectsDbConnection ?? throw new InvalidOperationException(
                "Connection string for projects DB wasn't found.");

            var optionsBuilder = new DbContextOptionsBuilder<ProjectsDataContext>();
            optionsBuilder.UseSqlite($"{DbConstants.SqLite.DataSourceParameterName}{connectionString}");

            return new ProjectsDataContext(optionsBuilder.Options);
        }
    }
}

using Microsoft.EntityFrameworkCore;
using App.Catalog.Db.Model;
using App.Catalog.Db.Model.Enums;

namespace App.Catalog.Db
{
    public class CatalogDbInitializer
    {
        private readonly IDbContextFactory<CatalogDbContext> _dbContextFactory;

        public CatalogDbInitializer(IDbContextFactory<CatalogDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        
        public void InitDataBase()
        {
            using var context = _dbContextFactory.CreateDbContext();
            context.Database.Migrate();

            if (context.FooCatalog.Any() || context.BarCatalog.Any())
            {
                return;
            }

            var powerPlus24Vdc = new PowerType
            {
                Type = PowerTypeEnum.PowerPlus24Vdc,
                Name = @"+24VDC"
            };

            var powerPlus12Vdc = new PowerType
            {
                Type = PowerTypeEnum.PowerPlus12Vdc,
                Name = @"+12VDC"
            };

            context.PowerTypes.AddRange(powerPlus24Vdc, powerPlus12Vdc);

            var foo =
                new FooCatalog
                {
                    PartNumber = "723778",
                    ModelNumber = "Foo Model 1",
                    IdGuid = new Guid("0117C24B-B01E-4D07-9FCC-654BA92E50CC"),
                    Version = 1,
                    FooFeature = "Feature Value 1",
                    PowerType = powerPlus12Vdc
                };

            var bar =
                new BarCatalog
                {
                    PartNumber = "545073",
                    ModelNumber = "Bar Model 1",
                    IdGuid = new Guid("5B480CB6-7CE1-4BE1-BA42-854111F17244"),
                    Version = 1,
                    BarFeature = "Feature Value 1",
                    PowerType = powerPlus24Vdc
                };

            context.FooCatalog.Add(foo);
            context.BarCatalog.Add(bar);

            var channel1 = new Channel
            {
                Type = ChannelTypeEnum.Temperature,
                Name = @"Channel1"
            };

            var channel2 = new Channel
            {
                Type = ChannelTypeEnum.Pressure,
                Name = @"Channel2"
            };

            context.Channel.Add(channel1);
            context.Channel.Add(channel2);

            context.FooCatalogChannel.Add(
                new FooCatalogChannel
                {
                    FooCatalog = foo,
                    Channel = channel1
                });

            context.BarCatalogChannel.Add(
                new BarCatalogChannel
                {
                    BarCatalog = bar,
                    Channel = channel2
                });

            context.SaveChanges();
        }
    }
}

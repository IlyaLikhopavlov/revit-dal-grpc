namespace App.Catalog.Db.Model
{
    public class BarCatalog : BaseCatalogEntity
    {
        public string BarFeature { get; set; }

        public string Description { get; set; }

        public ICollection<BarCatalogChannel> BarCatalogChannels { get; set; } = new List<BarCatalogChannel>();
    }
}

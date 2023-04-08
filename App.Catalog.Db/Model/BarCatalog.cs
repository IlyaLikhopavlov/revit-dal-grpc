using System.Text.Json.Serialization;

namespace App.Catalog.Db.Model
{
    public class BarCatalog : BaseCatalogEntity
    {
        public string BarFeature { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public int PowerTypeId { get; set; }

        public PowerType PowerType { get; set; }

        public ICollection<BarCatalogChannel> BarCatalogChannels { get; set; } = new List<BarCatalogChannel>();
    }
}

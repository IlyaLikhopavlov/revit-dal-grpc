using System.Text.Json.Serialization;

namespace App.Catalog.Db.Model
{
    [JsonDerivedType(typeof(BaseCatalogEntity), typeDiscriminator: "base")]
    [JsonDerivedType(typeof(FooCatalog), typeDiscriminator: "fooCatalog")]
    [JsonDerivedType(typeof(BarCatalog), typeDiscriminator: "barCatalog")]
    public class BaseCatalogEntity
    {
        [JsonIgnore]
        public int Id { get; set; }
        
        public long Version { get; set; }

        public Guid IdGuid { get; set; }

        public string PartNumber { get; set; }

        public string ModelNumber { get; set; }
    }
}

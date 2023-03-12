using System.Text.Json.Serialization;

namespace App.Catalog.Db.Model
{
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

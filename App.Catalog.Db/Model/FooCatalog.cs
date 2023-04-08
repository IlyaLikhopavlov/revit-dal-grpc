using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Catalog.Db.Model
{
    public class FooCatalog : BaseCatalogEntity
    {
        public string FooFeature { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public int PowerTypeId { get; set; }

        public PowerType PowerType { get; set; }

        public ICollection<FooCatalogChannel> FooCatalogChannels { get; set; } = new List<FooCatalogChannel>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using App.Catalog.Db.Model.Enums;

namespace App.Catalog.Db.Model
{
    public class Channel
    {
        [JsonIgnore]
        public int Id { get; set; }

        public ChannelTypeEnum Type { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<FooCatalogChannel> FooCatalogChannels { get; set; } = new List<FooCatalogChannel>();

        [JsonIgnore]
        public ICollection<BarCatalogChannel> BarCatalogChannels { get; set; } = new List<BarCatalogChannel>();
    }
}

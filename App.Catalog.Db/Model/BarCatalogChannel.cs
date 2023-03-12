using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Catalog.Db.Model
{
    public class BarCatalogChannel
    {
        [JsonIgnore]
        public int BarCatalogId { get; set; }

        public BarCatalog BarCatalog { get; set; }

        [JsonIgnore]
        public int ChannelId { get; set; }

        public Channel Channel { get; set; }
    }
}

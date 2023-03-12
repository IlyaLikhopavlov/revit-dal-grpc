using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Catalog.Db.Model
{
    public class FooCatalogChannel
    {
        [JsonIgnore]
        public int FooCatalogId { get; set; }

        public FooCatalog FooCatalog { get; set; }

        [JsonIgnore]
        public int ChannelId { get; set; }

        public Channel Channel { get; set; }
    }
}

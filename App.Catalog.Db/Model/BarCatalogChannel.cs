using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Catalog.Db.Model
{
    public class BarCatalogChannel
    {
        public int BarCatalogId { get; set; }

        public BarCatalog BarCatalog { get; set; }

        public int ChannelId { get; set; }

        public Channel Channel { get; set; }
    }
}

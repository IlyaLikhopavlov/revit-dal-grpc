using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Catalog.Db.Model
{
    public class FooCatalogChannel
    {
        public int FooCatalogId { get; set; }

        public FooCatalog FooCatalog { get; set; }

        public int ChannelId { get; set; }

        public Channel Channel { get; set; }
    }
}

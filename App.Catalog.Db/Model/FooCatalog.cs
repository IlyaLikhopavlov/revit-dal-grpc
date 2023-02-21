using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Catalog.Db.Model
{
    public class FooCatalog : BaseCatalogEntity
    {
        public string FooFeature { get; set; }

        public string Description { get; set; }
    }
}

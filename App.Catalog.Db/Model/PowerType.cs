using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using App.Catalog.Db.Model.Enums;

namespace App.Catalog.Db.Model
{
    public class PowerType
    {
        public int Id { get; set; }

        public PowerTypeEnum Type { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public virtual ICollection<FooCatalog> FooCatalogs { get; set; } = new List<FooCatalog>();

        [JsonIgnore]
        public virtual ICollection<BarCatalog> BarCatalogs { get; set; } = new List<BarCatalog>();
    }
}

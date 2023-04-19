using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DML
{
    public class BcLevel : BaseItem
    {
        public BcLevel(List<string> rooms) : this(rooms, null, null)
        {
        }

        public BcLevel(List<string> rooms, string elevation, string name) : base(name)
        {
            Rooms = rooms;
            Elevation = elevation;
        }

        public List<string> Rooms { get; set; }

        public string Elevation { get; set; }
    }
}
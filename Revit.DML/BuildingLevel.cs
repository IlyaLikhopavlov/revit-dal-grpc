using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.DML
{
    public class BuildingLevel : BaseItem
    {
        public BuildingLevel(List<Room> rooms) : this(rooms, null, null)
        {
        }

        public BuildingLevel(List<Room> rooms, string elevation, string name) : base(name)
        {
            Rooms = rooms;
            Elevation = elevation;
        }

        public List<Room> Rooms { get; set; }

        public string Elevation { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit.DML
{
    public class Bar : BaseEntity
    {
        public Bar() : this(null, null)
        {
        }

        public Bar(string? description, string? name) : base(name)
        {
            Description = description;
        }

        public string? Description { get; set; }
    }
}

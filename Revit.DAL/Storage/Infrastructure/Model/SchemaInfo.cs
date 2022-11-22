using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;

namespace Revit.DAL.Storage.Infrastructure.Model
{
    public class SchemaInfo
    {
        public Guid Guid { get; set; }

        public string Name { get; set; }

        public Type SchemaType { get; set; }

        public Type TargetType { get; set; }

        public Element TargetElement { get; set; }

        public string FieldName { get; set; }
    }
}

using Autodesk.Revit.DB;

namespace Revit.DAL.Storage.Infrastructure.Model
{
    public class SchemaInfo
    {
        public Guid Guid { get; set; }

        public string EntityName { get; set; }

        public Type SchemaType { get; set; }

        public Type TargetType { get; set; }

        public Element TargetElement { get; set; }

        public string FieldName { get; set; }
    }
}

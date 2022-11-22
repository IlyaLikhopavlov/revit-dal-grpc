using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.DAL.Storage.Infrastructure.Model;

namespace Revit.DAL.Storage.Infrastructure
{
    public class ExtensibleStorageArray<T> : ExtensibleStorageCollection<List<T>, IList<T>>
    {
        public ExtensibleStorageArray(SchemaDescriptor schemaDescriptor)
            : base(
                schemaDescriptor.Guid.ToString(),
                schemaDescriptor.Name,
                schemaDescriptor.FieldName,
                schemaDescriptor.TargetElement)
        {
        }

        protected override void AddField(SchemaBuilder schemaBuilder)
        {
            schemaBuilder.AddArrayField(FieldName, typeof(T));
        }
    }
}

using Autodesk.Revit.DB.ExtensibleStorage;
using Revit.Storage.ExtensibleStorage.Infrastructure.Model;

namespace Revit.Storage.ExtensibleStorage.Infrastructure
{
    public class ExtensibleStorageArray<T> : ExtensibleStorageCollection<List<T>, IList<T>>
    {
        public ExtensibleStorageArray(SchemaDescriptor schemaDescriptor)
            : base(
                schemaDescriptor.SchemaInfo.Guid.ToString(),
                schemaDescriptor.SchemaInfo.EntityName,
                schemaDescriptor.SchemaInfo.FieldName,
                schemaDescriptor.SchemaInfo.TargetElement)
        {
        }

        protected override void AddField(SchemaBuilder schemaBuilder)
        {
            schemaBuilder.AddArrayField(FieldName, typeof(T));
        }
    }
}

using Revit.Storage.ExtensibleStorage.Infrastructure.Model;
using Revit.Storage.ExtensibleStorage.Schemas;

namespace Revit.Storage.ExtensibleStorage.Infrastructure
{
    public class ExtensibleStorageDataSchema : ExtensibleStorage<DataSchema>, IExtensibleStorageDataSchema
    {
        public ExtensibleStorageDataSchema(SchemaDescriptor schemaDescriptor) : base(schemaDescriptor)
        {
        }
    }
}

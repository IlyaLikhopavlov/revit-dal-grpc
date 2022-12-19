using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

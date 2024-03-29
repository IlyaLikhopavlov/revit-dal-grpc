﻿using Revit.Storage.ExtensibleStorage.Infrastructure.Model;

namespace Revit.Storage.ExtensibleStorage
{
    public interface ISchemaDescriptorsRepository : IEnumerable<SchemaDescriptor>
    {
        SchemaDescriptor this[Guid guid] { get; }

        bool IsSchemaExists(Guid guid);

        SchemaDescriptor this[string entityName] { get; }
    }
}

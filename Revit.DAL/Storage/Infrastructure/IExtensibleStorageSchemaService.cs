using Autodesk.Revit.DB;
using Revit.DAL.Storage.Infrastructure.Model;

namespace Revit.DAL.Storage.Infrastructure;

public interface IExtensibleStorageSchemaService : IEnumerable<ExtensibleStorageSchemaDescriptor>
{
    void Register(ExtensibleStorageSchemaDescriptor descriptor);
    void Register(string guid, Type schemaType, Type targetType, string name, Element targetElement = null);
    ExtensibleStorageSchemaDescriptor this[Guid guid] { get; }
    bool IsSchemaExists (Guid guid);
}
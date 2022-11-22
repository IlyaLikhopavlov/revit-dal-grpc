using Autodesk.Revit.DB;
using Revit.DAL.Storage.Infrastructure.Model;

namespace Revit.DAL.Storage.Infrastructure;

public interface IExtensibleStorageService : IEnumerable<SchemaDescriptor>
{
    void Register(SchemaDescriptor descriptor);
    SchemaDescriptor this[Guid guid] { get; }
    bool IsSchemaExists (Guid guid);
    IExtensibleStorage this[string name] { get; }
}
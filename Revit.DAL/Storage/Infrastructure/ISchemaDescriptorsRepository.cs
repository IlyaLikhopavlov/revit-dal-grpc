using Revit.DAL.Storage.Infrastructure.Model;

namespace Revit.DAL.Storage.Infrastructure
{
    public interface ISchemaDescriptorsRepository : IEnumerable<SchemaDescriptor>
    {
        SchemaDescriptor this[Guid guid] { get; }

        bool IsSchemaExists(Guid guid);

        SchemaDescriptor this[string entityName] { get; }
    }
}

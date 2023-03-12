using Revit.Services.Grpc.Services;

namespace Revit.Storage.InstancesAccess
{
    public interface IRevitCatalogContext
    {
        void CreateOrUpdateRecord(CatalogRecordData data);

        bool Contains(string uniqueId);

        CatalogRecordData ReadById(string uniqueId);

        IEnumerable<CatalogRecordData> ReadAll();
    }
}

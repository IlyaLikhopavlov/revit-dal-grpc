using Revit.Services.Grpc.Services;

namespace Revit.Storage.InstancesAccess
{
    public interface IRevitCatalogContext
    {
        void CreateOrUpdateRecordInCatalog(CatalogRecordData data);

        bool Contains(string uniqueId);

        CatalogRecordData ReadFromCatalog(string uniqueId);
    }
}

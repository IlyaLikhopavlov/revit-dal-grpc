using Revit.Services.Grpc.Services;

namespace Revit.Storage.InstancesAccess
{
    public interface IRevitCatalogContext
    {
        void CreateOrUpdateRecordInCatalog(CatalogRecordData data);

        CatalogRecordData ReadFromCatalog(string guidId);
    }
}

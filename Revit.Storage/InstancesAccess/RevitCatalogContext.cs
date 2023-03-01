using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.Storage.ExtensibleStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Bimdance.Revit.Framework.RevitDocument;
using Revit.Services.Grpc.Services;
using Revit.Storage.ExtensibleStorage.Constants;

namespace Revit.Storage.InstancesAccess
{
    public class RevitCatalogContext : IRevitCatalogContext
    {
        private readonly IExtensibleStorageService _storageService;

        public RevitCatalogContext(IFactory<Document, IExtensibleStorageService> extensibleStorageServiceFactory, Document document)
        {
            _storageService = extensibleStorageServiceFactory.New(document);
        }

        public void CreateOrUpdateRecordInCatalog(CatalogRecordData data)
        {
            var storage = _storageService.GetDictionaryStorage(RevitStorage.Catalog.SchemaName);

            if (storage.Contains(data.GuidId))
            {
                storage.UpdateEntity(data.GuidId, data.Data);
            }
            else
            {
                storage.AddEntity(data.GuidId, data.Data);
            }
        }

        public CatalogRecordData ReadFromCatalog(string guidId)
        {
            var storage = _storageService.GetDictionaryStorage(RevitStorage.Catalog.SchemaName);
            var data = storage.GetEntity(guidId);

            return new CatalogRecordData
            {
                GuidId = guidId,
                Data = data
            };
        }
    }
}

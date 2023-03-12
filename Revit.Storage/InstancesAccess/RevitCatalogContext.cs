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

        private const string SchemaName = RevitStorage.Catalog.SchemaName;

        public RevitCatalogContext(IFactory<Document, IExtensibleStorageService> extensibleStorageServiceFactory, Document document)
        {
            _storageService = extensibleStorageServiceFactory.New(document);
        }

        public void CreateOrUpdateRecord(CatalogRecordData data)
        {
            var storage = _storageService.GetDictionaryStorage(SchemaName);

            if (storage.Contains(data.GuidId))
            {
                storage.UpdateEntity(data.GuidId, data.Data);
            }
            else
            {
                storage.AddEntity(data.GuidId, data.Data);
            }
        }

        public bool Contains(string uniqueId)
        {
            var storage = _storageService.GetDictionaryStorage(SchemaName);
            return storage.Contains(uniqueId);
        }

        public CatalogRecordData ReadById(string uniqueId)
        {
            var storage = _storageService.GetDictionaryStorage(SchemaName);
            var data = storage.GetEntity(uniqueId);

            return new CatalogRecordData
            {
                GuidId = uniqueId,
                Data = data
            };
        }

        public IEnumerable<CatalogRecordData> ReadAll()
        {
            var storage = _storageService.GetDictionaryStorage(SchemaName);
            return storage.GetAll()
                .Select(x => new CatalogRecordData
                {
                    GuidId = x.Key,
                    Data = x.Value
                });
            
        }
    }
}

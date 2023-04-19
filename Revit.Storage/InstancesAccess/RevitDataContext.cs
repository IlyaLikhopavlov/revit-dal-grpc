using System.Globalization;
using Autodesk.Revit.DB;
using Revit.Storage.ExtensibleStorage.Constants;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Revit.Framework.RevitDocument;
using Revit.Storage.ExtensibleStorage;
using Revit.Services.Grpc.Services;
using Revit.Storage.ExtensibleStorage.Schemas;
using GrpcLevel = Revit.Services.Grpc.Services.Level;
using GrpcRoom = Revit.Services.Grpc.Services.Room;
using Revit.Storage.RevitUtils;

namespace Revit.Storage.InstancesAccess
{
    public class RevitDataContext
    {
        private readonly Document _document;
        
        private readonly IExtensibleStorageService _storageService;

        public RevitDataContext(IFactory<Document, IExtensibleStorageService> extensibleStorageServiceFactory, Document document)
        {
            _document = document ?? throw new ArgumentException($"{nameof(document)} isn't initialized.");
            _storageService = extensibleStorageServiceFactory.New(document);
        }

        public void PushDataToRevitInstance(InstanceData data, DomainModelTypesEnum entityType)
        {
            var element = _document.GetElement(new ElementId(data.InstanceId));
            var storage = _storageService.GetDataSchemaStorage(entityType);

            _document.SaveChanges(() =>
            {
                storage.UpdateEntity(element, 
                    new DataSchema
                    {
                        Data = data.Data
                    });
            });
        }

        public InstanceData PullDataFromRevitInstance(int instanceId, DomainModelTypesEnum entityType)
        {
            var element = _document.GetElement(new ElementId(instanceId));
            var storage = _storageService.GetDataSchemaStorage(entityType);

            return new InstanceData
            {
                InstanceId = element.Id.IntegerValue,
                Data = storage.GetEntity(element)?.Data
            };
        }

        public IEnumerable<GrpcLevel> GetLevelsFromRevit()
        {
            var revitLevels  = _document.GetLevels();

            var grpsLevels = revitLevels.Select(x => new GrpcLevel
            {
                Value = x.Elevation.ToString(CultureInfo.InvariantCulture),
                Rooms = {_document.GetLevelRooms(x).Select(r=> new GrpcRoom
                {
                    Number = r.Number
                })}
            });

            return grpsLevels;
        }

        public void PushDataIntoInstancesById(IEnumerable<InstanceData> data, DomainModelTypesEnum entityType)
        {
            var storage = _storageService.GetDataSchemaStorage(entityType);
            var dataList = data.ToList();

            var elements = _document.GetInstancesByIdList(dataList.Select(x => x.InstanceId));

            _document.SaveChanges(() =>
            {
                var elementsWithData = dataList.Join(
                    elements,
                    instanceData => instanceData.InstanceId,
                    instance => instance.Id.IntegerValue,
                    (instanceData, instance) => new { Instance = instance, instanceData.Data });

                foreach (var item in elementsWithData)
                {
                    storage.UpdateEntity(item.Instance, new DataSchema { Data = item.Data });
                }
            });
        }

        public IEnumerable<InstanceData> PullDataFromInstancesByType(DomainModelTypesEnum entityType)
        {
            var instances = _document.GetInstancesByType(entityType);
            var storage = _storageService.GetDataSchemaStorage(entityType);

            return instances.Select(x => 
                new InstanceData
                {
                    Data = storage.GetEntity(x).Data,
                    InstanceId = x.Id.IntegerValue
                });
        }

        public int? CreateRevitElement(DomainModelTypesEnum entityType)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRevitElement(int instanceId)
        {
            var requiredId = new ElementId(instanceId);

            if (_document?.GetElement(requiredId) is null)
            {
                return false;
            }

            var deletedIds = Array.Empty<ElementId>();
            _document.SaveChanges(() =>
            {
                deletedIds = _document.Delete(requiredId).ToArray();
            });
            
            return deletedIds.Any(x => x.Equals(requiredId));
        }
    }
}

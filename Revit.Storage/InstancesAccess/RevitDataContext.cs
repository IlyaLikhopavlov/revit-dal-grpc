using Autodesk.Revit.DB;
using Revit.Storage.ExtensibleStorage.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.Storage.ExtensibleStorage;
using Bimdance.Framework.Exceptions;
using Revit.Services.Grpc.Services;
using Revit.Storage.ExtensibleStorage.Schemas;

namespace Revit.Storage.InstancesAccess
{
    public class RevitDataContext
    {
        private readonly Document _document;
        
        private readonly IExtensibleStorageService _storageService;

        protected RevitDataContext(Document document, IFactory<Document, IExtensibleStorageService> extensibleStorageServiceFactory)
        {
            _document = document ?? throw new ArgumentException($"{nameof(document)} isn't initialized.");
            _storageService = extensibleStorageServiceFactory.New(document);
        }

        public void PushDataToRevitInstance(InstanceData data, DomainModelTypesEnum entityType)
        {
            var element = _document.GetElement(new ElementId(data.InstanceId));
            var storage = _storageService.GetDataSchemaStorage(entityType);

            SaveChanges(() =>
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

        public void PushDataIntoInstancesById(IEnumerable<InstanceData> data, DomainModelTypesEnum entityType)
        {
            var storage = _storageService.GetDataSchemaStorage(entityType);
            var dataList = data.ToList();

            var elements = _document.GetInstancesByIdList(dataList.Select(x => x.InstanceId));

            SaveChanges(() =>
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
            SaveChanges(() =>
            {
                deletedIds = _document.Delete(requiredId).ToArray();
            });
            
            return deletedIds.Any(x => x.Equals(requiredId));
        }

        public void SaveChanges(Action sync, bool isInSubTransaction = false)
        {
            if (_document.IsReadOnly)
            {
                throw new InvalidOperationException($"Document {_document.Title} is read only. Changes can't be saved.");
            }

            if (!_document.IsModifiable && !isInSubTransaction)
            {
                using var transaction = new Transaction(_document, RevitStorage.SaveChangesTransactionName);
                try
                {
                    if (transaction.Start() == TransactionStatus.Started)
                    {
                        sync.Invoke();
                    }

                    if (TransactionStatus.Committed != transaction.Commit())
                    {
                        transaction.RollBack();
                    }
                }
                catch (Exception)
                {
                    transaction.RollBack();
                    throw;
                }
            }
            else
            {
                using var subTransaction = new SubTransaction(_document);
                try
                {
                    if (subTransaction.Start() == TransactionStatus.Started)
                    {
                        sync.Invoke();
                    }

                    if (TransactionStatus.Committed != subTransaction.Commit())
                    {
                        subTransaction.RollBack();
                    }
                }
                catch (Exception)
                {
                    subTransaction.RollBack();
                    throw;
                }
            }
        }
    }
}

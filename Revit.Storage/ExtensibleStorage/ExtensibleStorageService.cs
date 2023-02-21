using System.Collections.Concurrent;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.Services.Grpc.Services;
using Revit.Storage.ExtensibleStorage.Infrastructure;
using Revit.Storage.ExtensibleStorage.Infrastructure.Model;
using Revit.Storage.ExtensibleStorage.Schemas;

namespace Revit.Storage.ExtensibleStorage
{

    //todo dispose dependencies
    public class ExtensibleStorageService : IExtensibleStorageService
    {
        private readonly ConcurrentDictionary<string, IExtensibleStorage> _extensibleStorages = new();

        private readonly IFactory<SchemaDescriptor, IExtensibleStorageDataSchema>
            _dataExtensibleStorageFactory;

        private readonly IFactory<SchemaDescriptor, IExtensibleStorageDictionary>
            _dictionaryExtensibleStorageFactory;

        private readonly IFactory<SchemaDescriptor, IIntIdGenerator> _idGeneratorFactory;

        public ExtensibleStorageService(
            IFactory<SchemaDescriptor, IExtensibleStorageDataSchema> dataExtensibleStorageFactory,
            IFactory<SchemaDescriptor, IExtensibleStorageDictionary> dictionaryExtensibleStorageFactory,
            IFactory<SchemaDescriptor, IIntIdGenerator> idGeneratorFactory,
            IFactory<Document, ISchemaDescriptorsRepository> repositoryFactory,
            Document document)
        {
            _dataExtensibleStorageFactory = dataExtensibleStorageFactory;
            _dictionaryExtensibleStorageFactory = dictionaryExtensibleStorageFactory;
            _idGeneratorFactory = idGeneratorFactory;
            var schemaDescriptorsRepository = repositoryFactory.New(document);

            foreach (var descriptor in schemaDescriptorsRepository)
            {
                Register(descriptor);
            }
        }

        private void Register(SchemaDescriptor descriptor)
        {
            if (descriptor.SchemaInfo.SchemaType == typeof(DataSchema))
            {
                _extensibleStorages.TryAdd(descriptor.SchemaInfo.EntityName, _dataExtensibleStorageFactory.New(descriptor));
                return;
            }

            if (descriptor.SchemaInfo.SchemaType == typeof(IDictionary<string, string>))
            {
                _extensibleStorages.TryAdd(descriptor.SchemaInfo.EntityName, _dictionaryExtensibleStorageFactory.New(descriptor));
                return;
            }

            if (descriptor.SchemaInfo.SchemaType == typeof(IList<int>))
            {
                _extensibleStorages.TryAdd(descriptor.SchemaInfo.EntityName, _idGeneratorFactory.New(descriptor));
            }
        }

        public IExtensibleStorageDataSchema GetDataSchemaStorage(DomainModelTypesEnum entityType)
        {
            return this[entityType.ToString()] as IExtensibleStorageDataSchema
                ?? throw new ArgumentException($"Storage with provided entity type {entityType} wasn't founded.");
        }

        public IExtensibleStorageDictionary GetDictionaryStorage(string entityName)
        {
            return this[entityName] as IExtensibleStorageDictionary 
                   ?? throw new ArgumentException($"Storage with provided entity name {entityName} wasn't founded.");
        }

        public IIntIdGenerator GetIdGenerator(string entityName)
        {
            return this[entityName] as IIntIdGenerator 
                   ?? throw new ArgumentException($"Storage with provided entity name {entityName} wasn't founded.");
        }

        public IExtensibleStorage this[string entityName]
        {
            get
            {
                if (_extensibleStorages.TryGetValue(entityName, out var extensibleStorage))
                {
                    return extensibleStorage;
                }

                throw new ArgumentException($"Extensible storage hasn't found by name {entityName}");
            }
        }

        public static string GetData(Entity entity, SchemaDescriptor descriptor)
        {
            var getMethod = typeof(Entity).GetMethod(nameof(Entity.Get), new[] { typeof(string) });
            var getGeneric = getMethod?.MakeGenericMethod(descriptor.GetType());
            var result = getGeneric?.Invoke(entity, new object[] { descriptor.SchemaInfo.FieldName });

            if (descriptor.FieldType == typeof(string))
            {
                return (string)result;
            }

            return descriptor.FormatData(result);
        }
    }
}

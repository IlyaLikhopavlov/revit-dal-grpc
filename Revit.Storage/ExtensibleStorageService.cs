using System.Collections.Concurrent;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.Storage.Infrastructure;
using Revit.Storage.Infrastructure.Model;
using Revit.Storage.Schemas;

namespace Revit.Storage
{

    //todo dispose dependencies
    public class ExtensibleStorageService : IExtensibleStorageService
    {
        private readonly ConcurrentDictionary<string, IExtensibleStorage> _extensibleStorages = new();

        private readonly IFactory<SchemaDescriptor, ExtensibleStorage<DataSchema>>
            _dataExtensibleStorageFactory;

        private readonly IFactory<SchemaDescriptor, ExtensibleStorageDictionary>
            _dictionaryExtensibleStorageFactory;

        private readonly IFactory<SchemaDescriptor, IIntIdGenerator> _idGeneratorFactory;

        public ExtensibleStorageService(
            IFactory<SchemaDescriptor, ExtensibleStorage<DataSchema>> dataExtensibleStorageFactory,
            IFactory<SchemaDescriptor, ExtensibleStorageDictionary> dictionaryExtensibleStorageFactory,
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

        public IExtensibleStorage this[string name]
        {
            get
            {
                if (_extensibleStorages.TryGetValue(name, out var extensibleStorage))
                {
                    return extensibleStorage;
                }

                throw new ArgumentException($"Extensible storage hasn't found by name {name}");
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

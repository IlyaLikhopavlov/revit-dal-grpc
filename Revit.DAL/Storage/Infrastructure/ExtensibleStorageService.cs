using System.Collections;
using System.Collections.Concurrent;
using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Storage.Infrastructure.Model;
using Revit.DAL.Storage.Schemas;

namespace Revit.DAL.Storage.Infrastructure
{
    //todo it needs to change to a factory for Extensible Storages
    public class ExtensibleStorageService : IExtensibleStorageService
    {
        private readonly ConcurrentDictionary<Guid, SchemaDescriptor> _schemaDescriptors = new();

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
            IFactory<Document, SchemaDescriptorsRepository> repositoryFactory,
            Document document)
        {
            _dataExtensibleStorageFactory = dataExtensibleStorageFactory;
            _dictionaryExtensibleStorageFactory = dictionaryExtensibleStorageFactory;
            _idGeneratorFactory = idGeneratorFactory;
            var repository = repositoryFactory.New(document);

            ////registering
            foreach (var descriptor in repository.Descriptors)
            {
                Register(descriptor);
            }
        }

        public void Register(SchemaDescriptor descriptor)
        {
            if (_schemaDescriptors.Values.Any(x => x.Name == descriptor.Name))
            {
                throw new InvalidOperationException("Schema name must be unique");
            }
            
            if (descriptor.SchemaType == typeof(IDictionary<string, string>))
            {
                descriptor.Converter = o =>
                {
                    var dictionary = (IDictionary<string, string>)o;
                    return "[" + string.Join(",",
                        dictionary.Select(kv => "{\"" + kv.Key + "\"" + ": " + kv.Value + "}").ToArray()) + "]";
                };
            }

            if (descriptor.SchemaType == typeof(IList<int>))
            {
                descriptor.Converter = o =>
                {
                    var list = (IList<int>)o;
                    return "[" + string.Join(",", list) + "]";
                };
            }

            if (!_schemaDescriptors.TryAdd(descriptor.Guid, descriptor))
            {
                throw new ArgumentException($"Scheme descriptor has registered already {descriptor.Guid}");
            }

            if (descriptor.SchemaType == typeof(DataSchema))
            {
                _extensibleStorages.TryAdd(descriptor.Name, _dataExtensibleStorageFactory.New(descriptor));
                return;
            }

            if (descriptor.SchemaType == typeof(IDictionary<string, string>))
            {
                _extensibleStorages.TryAdd(descriptor.Name, _dictionaryExtensibleStorageFactory.New(descriptor));
                return;
            }

            if (descriptor.SchemaType == typeof(IList<int>))
            {
                _extensibleStorages.TryAdd(descriptor.Name, _idGeneratorFactory.New(descriptor));
            }
        }

        public SchemaDescriptor this[Guid guid]
        {
            get
            {
                if (_schemaDescriptors.TryGetValue(guid, out var schemaDescriptor))
                {
                    return new SchemaDescriptor(schemaDescriptor)
                    {
                        Converter = schemaDescriptor.Converter
                    };
                }

                throw new ArgumentException($"Scheme descriptor hasn't found by GUID {guid}.");
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

        public bool IsSchemaExists(Guid guid)
        {
            return _schemaDescriptors.ContainsKey(guid);
        }

        public IEnumerator<SchemaDescriptor> GetEnumerator() =>
            _schemaDescriptors.Values
                .Select(d => new SchemaDescriptor(d)
                {
                    Converter = d.Converter
                })
                .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

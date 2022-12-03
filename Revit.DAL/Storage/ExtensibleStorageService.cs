using System.Collections.Concurrent;
using System.Text.Json;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Storage.Infrastructure;
using Revit.DAL.Storage.Infrastructure.Model;
using Revit.DAL.Storage.Infrastructure.Model.Enums;
using Revit.DAL.Storage.Schemas;

namespace Revit.DAL.Storage
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

        private readonly ISchemaDescriptorsRepository _schemaDescriptorsRepository;

        private readonly Document _document;

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
            _document = document;
            _schemaDescriptorsRepository = repositoryFactory.New(_document);

            foreach (var descriptor in _schemaDescriptorsRepository)
            {
                Register(descriptor);
            }
        }

        private void Register(SchemaDescriptor descriptor)
        {
            if (descriptor.SchemaType == typeof(DataSchema))
            {
                _extensibleStorages.TryAdd(descriptor.EntityName, _dataExtensibleStorageFactory.New(descriptor));
                return;
            }

            if (descriptor.SchemaType == typeof(IDictionary<string, string>))
            {
                _extensibleStorages.TryAdd(descriptor.EntityName, _dictionaryExtensibleStorageFactory.New(descriptor));
                return;
            }

            if (descriptor.SchemaType == typeof(IList<int>))
            {
                _extensibleStorages.TryAdd(descriptor.EntityName, _idGeneratorFactory.New(descriptor));
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
            var result = getGeneric?.Invoke(entity, new object[] { descriptor.FieldName });

            if (descriptor.FieldType == typeof(string))
            {
                return (string)result;
            }

            return descriptor.FormatData(result);
        }

        public IEnumerable<(string data, SchemaDescriptor descriptor)> GetProjectData()
        {
            var schemas = _schemaDescriptorsRepository
                .Where(x => x.Kind == TargetObjectKindEnum.ProjectInfo)
                .Select(x => new { Schema = Schema.Lookup(x.Guid), Descriptor = x })
                .Where(x => x.Schema is not null);

            return
                schemas
                    .Select(x =>
                    {
                        var entity = _document.ProjectInformation.GetEntity(x.Schema);

                        if (!entity.IsValid())
                        {
                            return (null, null);
                        }

                        var result = GetData(entity, x.Descriptor);
                        using var jDoc = JsonDocument.Parse(result);

                        return (JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true }), x.Descriptor);
                    })
                    .Where(x => x.Item1 is not null);
        }

        public (string data, SchemaDescriptor descriptor) GetDataFromElement(Element element)
        {
            var schemas = Schema.ListSchemas()
                .Where(x => _schemaDescriptorsRepository.IsSchemaExists(x.GUID));

            foreach (var schema in schemas)
            {
                var entity = element.GetEntity(schema);

                if (!entity.IsValid())
                {
                    continue;
                }

                var result = (string)ExtensibleStorageUtils.GetEntityFieldValue(entity, typeof(string), nameof(DataSchema.Data));
                using var jDoc = JsonDocument.Parse(result);

                return
                    (JsonSerializer.Serialize(jDoc, new JsonSerializerOptions { WriteIndented = true }),
                    _schemaDescriptorsRepository[entity.SchemaGUID]);
            }

            return (null, null);
        }
    }
}

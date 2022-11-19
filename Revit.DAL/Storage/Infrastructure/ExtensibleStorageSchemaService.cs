using System.Collections;
using System.Collections.Concurrent;
using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Constants;
using Revit.DAL.Storage.Infrastructure.Model;
using Revit.DAL.Storage.Schemas;
using Revit.DAL.Utils;

namespace Revit.DAL.Storage.Infrastructure
{
    //todo it needs to change to a factory for Extensible Storages
    public class ExtensibleStorageSchemaService : IExtensibleStorageSchemaService
    {
        private readonly ConcurrentDictionary<Guid, ExtensibleStorageSchemaDescriptor> _schemaDescriptors = new();

        public ExtensibleStorageSchemaService(IFactory<Guid, Type, Type, string, ExtensibleStorageSchemaDescriptor> descriptorsFactory)
        {
            var descriptors = new[]
            {
                descriptorsFactory.New(
                    new Guid(RevitStorage.FooSchemaGuid),
                    typeof(FooSchema),
                    typeof(FamilyInstance),
                    nameof(IDataSchema.Data)),

                descriptorsFactory.New(
                    new Guid(RevitStorage.BarSchemaGuid),
                    typeof(BarSchema),
                    typeof(FamilyInstance),
                    nameof(IDataSchema.Data)),

                descriptorsFactory.New(
                    new Guid(RevitStorage.SettingsExtensibleStorageSchemaGuid),
                    typeof(IDictionary<string, string>),
                    typeof(ProjectInfo),
                    "SettingsDictionary"),

                descriptorsFactory.New(
                    new Guid(RevitStorage.IdStorageSchemaGuid),
                    typeof(IList<int>),
                    typeof(ProjectInfo),
                    "IdList"),
            };
            
            descriptors
                .Where(x => x.SchemaType == typeof(IDictionary<string, string>))
                .ForEach(x => x.Converter = o =>
                {
                    var dictionary = (IDictionary<string, string>)o;
                    return "[" + string.Join(",", dictionary.Select(kv => "{\"" + kv.Key + "\"" + ": " + kv.Value + "}").ToArray()) + "]";
                });

            descriptors
                .Where(x => x.SchemaType == typeof(IList<int>))
                .ForEach(x => x.Converter = o =>
                {
                    var list = (IList<int>)o;
                    return "[" + string.Join(",", list) + "]";
                });

            foreach (var descriptor in descriptors)
            {
                Register(descriptor);
            }
        }

        public void Register(ExtensibleStorageSchemaDescriptor descriptor)
        {
            if (!_schemaDescriptors.TryAdd(descriptor.Guid, descriptor))
            {
                throw new ArgumentException($"Scheme descriptor has registered already {descriptor.Guid}");
            }
        }

        public void Register(string guid, Type schemaType, Type targetType, string name, Element targetElement = null)
        {
            Register(new ExtensibleStorageSchemaDescriptor(new Guid(guid), schemaType, targetType, name));
        }

        public ExtensibleStorageSchemaDescriptor this[Guid guid]
        {
            get
            {
                if (_schemaDescriptors.TryGetValue(guid, out var schemaDescriptor))
                {
                    return new ExtensibleStorageSchemaDescriptor(schemaDescriptor)
                    {
                        Converter = schemaDescriptor.Converter
                    };
                }

                throw new ArgumentException($"Scheme descriptor hasn't found by GUID {guid}.");
            }
        }

        public bool IsSchemaExists(Guid guid)
        {
            return _schemaDescriptors.ContainsKey(guid);
        }

        public IEnumerator<ExtensibleStorageSchemaDescriptor> GetEnumerator() =>
            _schemaDescriptors.Values
                .Select(d => new ExtensibleStorageSchemaDescriptor(d)
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

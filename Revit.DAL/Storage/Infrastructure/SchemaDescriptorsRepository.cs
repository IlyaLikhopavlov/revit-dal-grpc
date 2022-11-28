using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Constants;
using Revit.DAL.Storage.Infrastructure.Model;
using Revit.DAL.Storage.Schemas;
using Revit.DML;

namespace Revit.DAL.Storage.Infrastructure
{
    public class SchemaDescriptorsRepository : IEnumerable<SchemaDescriptor>
    {
        private readonly ConcurrentDictionary<Guid, SchemaDescriptor> _descriptorsDictionary;

        public SchemaDescriptorsRepository(IFactory<SchemaInfo, SchemaDescriptor> descriptorsFactory, Document document)
        {
            var descriptors = new []
            {
                descriptorsFactory.New(
                        new SchemaInfo {
                            Guid = new Guid(RevitStorage.FooSchemaGuid),
                            SchemaName = nameof(Foo),
                            SchemaType = typeof(DataSchema),
                            TargetType = typeof(FamilyInstance),
                            FieldName = nameof(DataSchema.Data)
                        }),

                    descriptorsFactory.New(
                        new SchemaInfo {
                            Guid = new Guid(RevitStorage.BarSchemaGuid),
                            SchemaName = nameof(Bar),
                            SchemaType = typeof(DataSchema),
                            TargetType = typeof(FamilyInstance),
                            FieldName = nameof(DataSchema.Data)
                        }),

                    descriptorsFactory.New(
                        new SchemaInfo
                        {
                            Guid = new Guid(RevitStorage.SettingsExtensibleStorageSchemaGuid),
                            SchemaName = RevitStorage.Settings.SchemaName,
                            SchemaType = typeof(IDictionary<string, string>),
                            TargetType = typeof(ProjectInfo),
                            FieldName = RevitStorage.Settings.FieldName,
                            TargetElement = document.ProjectInformation
                        }),

                    descriptorsFactory.New(
                        new SchemaInfo
                        {
                            Guid = new Guid(RevitStorage.IdStorageSchemaGuid),
                            SchemaName = RevitStorage.IdList.SchemaName,
                            SchemaType = typeof(IList<int>),
                            TargetType = typeof(ProjectInfo),
                            FieldName = RevitStorage.IdList.FieldName,
                            TargetElement = document.ProjectInformation
                        }),
            };

            if (descriptors.GroupBy(x => x.Name).Any(x => x.Count() > 1) || 
                descriptors.Any(x => string.IsNullOrWhiteSpace(x.Name)))
            {
                throw new InvalidOperationException("Schema name must be unique");
            }

            _descriptorsDictionary = new ConcurrentDictionary<Guid, SchemaDescriptor>(
                descriptors.ToDictionary(x => x.Guid, y => y));
        }

        public SchemaDescriptor this[Guid guid]
        {
            get
            {
                if (_descriptorsDictionary.TryGetValue(guid, out var schemaDescriptor))
                {
                    return new SchemaDescriptor(schemaDescriptor);
                }

                throw new ArgumentException($"Schema descriptor hasn't found by GUID {guid}.");
            }
        }

        public bool IsSchemaExists(Guid guid)
        {
            return _descriptorsDictionary.ContainsKey(guid);
        }

        public IEnumerator<SchemaDescriptor> GetEnumerator()
        {
            return
                _descriptorsDictionary.Values
                    .Select(x => new SchemaDescriptor(x))
                    .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

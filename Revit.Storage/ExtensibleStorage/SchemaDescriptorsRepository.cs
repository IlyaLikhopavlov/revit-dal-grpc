﻿using System.Collections;
using System.Collections.Concurrent;
using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.Services.Grpc.Services;
using Revit.Storage.ExtensibleStorage.Constants;
using Revit.Storage.ExtensibleStorage.Infrastructure.Model;
using Revit.Storage.ExtensibleStorage.Schemas;
using ArgumentException = System.ArgumentException;
using InvalidOperationException = System.InvalidOperationException;

namespace Revit.Storage.ExtensibleStorage
{
    public class SchemaDescriptorsRepository : ISchemaDescriptorsRepository
    {
        private readonly ConcurrentDictionary<Guid, SchemaDescriptor> _descriptorsDictionary;

        public SchemaDescriptorsRepository(IFactory<SchemaInfo, SchemaDescriptor> descriptorsFactory, Document document)
        {
            var descriptors = new[]
            {
                descriptorsFactory.New(
                    new SchemaInfo {
                        Guid = new Guid(RevitStorage.FooSchemaGuid),
                        EntityName = DomainModelTypesEnum.Foo.ToString(),
                        DomainModelType = DomainModelTypesEnum.Foo,
                        SchemaType = typeof(DataSchema),
                        TargetType = typeof(FamilyInstance),
                        FieldName = nameof(DataSchema.Data)
                    }),

                descriptorsFactory.New(
                    new SchemaInfo {
                        Guid = new Guid(RevitStorage.BarSchemaGuid),
                        EntityName = DomainModelTypesEnum.Bar.ToString(),
                        DomainModelType = DomainModelTypesEnum.Bar,
                        SchemaType = typeof(DataSchema),
                        TargetType = typeof(FamilyInstance),
                        FieldName = nameof(DataSchema.Data)
                    }),

                descriptorsFactory.New(
                    new SchemaInfo
                    {
                        Guid = new Guid(RevitStorage.SettingsExtensibleStorageSchemaGuid),
                        EntityName = RevitStorage.Settings.SchemaName,
                        SchemaType = typeof(IDictionary<string, string>),
                        TargetType = typeof(ProjectInfo),
                        FieldName = RevitStorage.Settings.FieldName,
                        TargetElement = document.ProjectInformation
                    }),

                descriptorsFactory.New(
                    new SchemaInfo
                    {
                        Guid = new Guid(RevitStorage.CatalogExtensibleStorageSchemaGuid),
                        EntityName = RevitStorage.Catalog.SchemaName,
                        SchemaType = typeof(IDictionary<string, string>),
                        TargetType = typeof(ProjectInfo),
                        FieldName = RevitStorage.Catalog.FieldName,
                        TargetElement = document.ProjectInformation
                    }),

                descriptorsFactory.New(
                    new SchemaInfo
                    {
                        Guid = new Guid(RevitStorage.IdStorageSchemaGuid),
                        EntityName = RevitStorage.IdList.SchemaName,
                        SchemaType = typeof(IList<int>),
                        TargetType = typeof(ProjectInfo),
                        FieldName = RevitStorage.IdList.FieldName,
                        TargetElement = document.ProjectInformation
                    }),
            };

            if (descriptors.GroupBy(x => x.SchemaInfo.EntityName).Any(x => x.Count() > 1) ||
                descriptors.Any(x => string.IsNullOrWhiteSpace(x.SchemaInfo.EntityName)))
            {
                throw new InvalidOperationException("Schema name must be unique and not empty.");
            }

            _descriptorsDictionary = new ConcurrentDictionary<Guid, SchemaDescriptor>(
                descriptors.ToDictionary(x => x.SchemaInfo.Guid, y => y));
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

        public SchemaDescriptor this[string entityName]
        {
            get
            {
                if (string.IsNullOrWhiteSpace(entityName))
                {
                    throw new ArgumentNullException(nameof(entityName));
                }

                return _descriptorsDictionary.Values.FirstOrDefault(x => entityName == x.SchemaInfo.EntityName) ??
                       throw new ArgumentException(nameof(entityName));
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

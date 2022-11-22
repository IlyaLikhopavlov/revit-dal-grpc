using System;
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
    public class SchemaDescriptorsRepository
    {
        private readonly IEnumerable<SchemaDescriptor> _descriptors;

        public SchemaDescriptorsRepository(IFactory<SchemaInfo, SchemaDescriptor> descriptorsFactory, Document document)
        {
            _descriptors = new []
            {
                descriptorsFactory.New(
                        new SchemaInfo {
                            Guid = new Guid(RevitStorage.FooSchemaGuid),
                            Name = nameof(Foo),
                            SchemaType = typeof(DataSchema),
                            TargetType = typeof(FamilyInstance),
                            FieldName = nameof(IDataSchema.Data)
                        }),

                    descriptorsFactory.New(
                        new SchemaInfo {
                            Guid = new Guid(RevitStorage.BarSchemaGuid),
                            Name = nameof(Bar),
                            SchemaType = typeof(DataSchema),
                            TargetType = typeof(FamilyInstance),
                            FieldName = nameof(IDataSchema.Data)
                        }),

                    descriptorsFactory.New(
                        new SchemaInfo
                        {
                            Guid = new Guid(RevitStorage.SettingsExtensibleStorageSchemaGuid),
                            Name = @"SettingsSchema",
                            SchemaType = typeof(IDictionary<string, string>),
                            TargetType = typeof(ProjectInfo),
                            FieldName = "SettingsDictionary",
                            TargetElement = document.ProjectInformation
                        }),

                    descriptorsFactory.New(
                        new SchemaInfo
                        {
                            Guid = new Guid(RevitStorage.IdStorageSchemaGuid),
                            Name = @"IdListSchema",
                            SchemaType = typeof(IList<int>),
                            TargetType = typeof(ProjectInfo),
                            FieldName = "IdList",
                            TargetElement = document.ProjectInformation
                        }),
            };
        }

        public IReadOnlyList<SchemaDescriptor> Descriptors => new List<SchemaDescriptor>(_descriptors);
    
    }
}

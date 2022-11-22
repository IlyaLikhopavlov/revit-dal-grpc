using System.Text.Json;
using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Converters.Common;
using Revit.DAL.Storage.Infrastructure;
using Revit.DAL.Storage.Schemas;
using Revit.DML;

namespace Revit.DAL.Converters
{
    public class FooConverter : RevitInstanceConverter<Foo, FamilyInstance>
    {
        private readonly ExtensibleStorage<DataSchema> _extensibleStorage;

        public FooConverter(
            IFactory<Document, IExtensibleStorageService> extensibleStorageFactory,
            Document document)
        {
            _extensibleStorage = (ExtensibleStorage<DataSchema>)extensibleStorageFactory.New(document)[ModelElementName];
        }

        public override void PushToRevit(FamilyInstance revitElement, Foo modelElement)
        {
            if (!string.IsNullOrEmpty(modelElement.Name) && revitElement.Name != modelElement.Name)
            {
                revitElement.Name = modelElement.Name;
            }

            //How to send data to family instance parameters?
            //var param1 = revitElement.LookupParameter(ParametersNames.Param1);
            //param1.SetValueString("Parameter Value");

            var schema = new DataSchema
            {
                Data = JsonSerializer.Serialize(modelElement)
            };

            _extensibleStorage.UpdateEntity(revitElement, schema);
        }

        public override Foo PullFromRevit(FamilyInstance revitElement)
        {
            var fooEntity = _extensibleStorage.GetEntity(revitElement);

            if (fooEntity.Data == null)
            {
                return null;
            }

            var foo = JsonSerializer.Deserialize<Foo>(fooEntity.Data);

            if (foo is null)
            {
                return null;
            }
            
            foo.Id = revitElement.Id.IntegerValue;

            return foo;
        }
    }
}

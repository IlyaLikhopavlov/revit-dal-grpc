using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.DAL.Storage.Infrastructure;
using Revit.DAL.Storage.Schemas;
using Revit.DML;
using System.Text.Json;
using System.Text.Json.Serialization;
using Element = Revit.DML.Element;

namespace Revit.DAL.Converters.Common
{
    public abstract class RevitInstanceConverter<TModelElement, TRevitElement> 
        where TModelElement : Element
        where TRevitElement : Autodesk.Revit.DB.Element
    {
        protected readonly ExtensibleStorage<DataSchema> ExtensibleStorage;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new();

        protected RevitInstanceConverter(IFactory<Document, IExtensibleStorageService> extensibleStorageFactory,
            Document document)
        {
            ExtensibleStorage = (ExtensibleStorage<DataSchema>)extensibleStorageFactory.New(document)[ModelElementName];
        }

        protected IList<JsonConverter> Converters => _jsonSerializerOptions.Converters;

        protected abstract void SendParametersToRevit(TRevitElement revitElement, TModelElement modelElement);

        protected abstract void ReceiveParametersFromRevit(TRevitElement revitElement, ref TModelElement modelElement);

        public virtual void PushToRevit(TRevitElement revitElement, TModelElement modelElement)
        {
            if (!string.IsNullOrEmpty(modelElement.Name) && revitElement.Name != modelElement.Name)
            {
                revitElement.Name = modelElement.Name;
            }

            SendParametersToRevit(revitElement, modelElement);

            var schema = new DataSchema
            {
                Data = JsonSerializer.Serialize(modelElement, _jsonSerializerOptions)
            };

            ExtensibleStorage.UpdateEntity(revitElement, schema);
        }

        public virtual TModelElement PullFromRevit(TRevitElement revitElement)
        {
            var entity = ExtensibleStorage.GetEntity(revitElement);

            if (entity.Data == null)
            {
                return null;
            }

            var modelElement = JsonSerializer.Deserialize<TModelElement>(entity.Data, _jsonSerializerOptions);

            ReceiveParametersFromRevit(revitElement, ref modelElement);

            if (modelElement is null)
            {
                return null;
            }

            modelElement.Id = revitElement.Id.IntegerValue;

            return modelElement;
        }

        public string ModelElementName => typeof(TModelElement).Name;
    }
}

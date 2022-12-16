
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using System.Text.Json;
using System.Text.Json.Serialization;
using Revit.Services.Grpc.Services;
using Element = Revit.DML.Element;

namespace Revit.DAL.Converters.Common
{
    public abstract class RevitInstanceConverter<TModelElement> 
        where TModelElement : Element
    {
        //protected readonly ExtensibleStorage<DataSchema> ExtensibleStorage;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new();

        protected RevitInstanceConverter(/*IFactory<Document, IExtensibleStorageService> extensibleStorageFactory*/
            DocumentDescriptor documentDescriptor)
        {
            //ExtensibleStorage = (ExtensibleStorage<DataSchema>)extensibleStorageFactory.New(document)[ModelElementName];
        }

        protected IList<JsonConverter> JsonConverters => _jsonSerializerOptions.Converters;

        protected abstract void PushParametersToRevit(TModelElement modelElement);

        protected abstract void PullParametersFromRevit(ref TModelElement modelElement);

        public virtual void PushToRevit(TModelElement modelElement)
        {
            //if (!string.IsNullOrEmpty(modelElement.Name) && revitElement.Name != modelElement.Name)
            //{
            //    revitElement.Name = modelElement.Name;
            //}

            PushParametersToRevit(modelElement);

            var schema = new DataSchema
            {
                Data = JsonSerializer.Serialize(modelElement, _jsonSerializerOptions)
            };

            //ExtensibleStorage.UpdateEntity(revitElement, schema);
        }

        public virtual TModelElement PullFromRevit()
        {
            //var entity = ExtensibleStorage.GetEntity(revitElement);

            //if (entity.Data == null)
            //{
            //    return null;
            //}

            //var modelElement = JsonSerializer.Deserialize<TModelElement>(entity.Data, _jsonSerializerOptions);

            //PullParametersFromRevit(ref modelElement);

            //if (modelElement is null)
            //{
            //    return null;
            //}

            //modelElement.Id = revitElement.Id.IntegerValue;

            //return modelElement;
            throw new NotImplementedException();
        }

        public string ModelElementName => typeof(TModelElement).Name;
    }
}

using System.Text.Json;
using System.Text.Json.Serialization;
using App.Services.Grpc;
using Revit.Services.Grpc.Services;
using static Grpc.Core.Metadata;
using Element = App.DML.Element;

namespace App.DAL.Converters.Common
{
    public abstract class RevitInstanceConverter<TModelElement> 
        where TModelElement : Element
    {
        protected readonly RevitExtraDataExchangeClient Client;

        protected readonly DocumentDescriptor DocumentDescriptor;

        private readonly JsonSerializerOptions _jsonSerializerOptions = new();

        protected RevitInstanceConverter(RevitExtraDataExchangeClient client,
            DocumentDescriptor documentDescriptor)
        {
            Client = client;
            DocumentDescriptor = documentDescriptor;
        }

        protected IList<JsonConverter> JsonConverters => _jsonSerializerOptions.Converters;

        protected abstract void PushParametersToRevit(TModelElement modelElement);

        protected abstract void PullParametersFromRevit(ref TModelElement modelElement);

        public virtual async Task PushToRevit(TModelElement modelElement)
        {
            //PushParametersToRevit(modelElement);

            await Client.PushDataToRevitInstance(
                typeof(TModelElement), 
                DocumentDescriptor,
                new InstanceData
                {
                    Data = JsonSerializer.Serialize(modelElement, _jsonSerializerOptions),
                    InstanceId = modelElement.Id
                });
        }

        public virtual async Task<TModelElement> PullFromRevit(int instanceId)
        {
            var data = await Client.PullDataFromRevitInstance(typeof(TModelElement), DocumentDescriptor, instanceId);
            var modelElement = JsonSerializer.Deserialize<TModelElement>(data, _jsonSerializerOptions);

            //PullParametersFromRevit(ref modelElement);

            return modelElement;
        }

        public virtual async Task<IEnumerable<TModelElement>> PullWholeFromRevit()
        {
            var data = await Client.PullDataFromRevitInstancesByType(typeof(TModelElement), DocumentDescriptor);
            return data.Select(x =>
                JsonSerializer.Deserialize<TModelElement>(x.Data, _jsonSerializerOptions)).ToArray();
        }

        public virtual async Task<int> CreateRevitElement()
        {
            return await Client.CreateRevitElement(typeof(TModelElement), DocumentDescriptor);
        }

        public virtual async Task<bool> DeleteRevitElement(int instanceId)
        {
            return await Client.DeleteRevitElement(instanceId, DocumentDescriptor);
        }

        public string ModelElementName => typeof(TModelElement).Name;
    }
}

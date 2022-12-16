using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Revit.DML;
using Revit.Services.Grpc.Services;

namespace App.Services.Grpc
{
    public class RevitExtraDataExchangeClient
    {
        private static readonly Dictionary<Type, DomainModelTypesEnum> TypesMapping = new()
        {
            { typeof(Foo), DomainModelTypesEnum.Foo },
            { typeof(Bar), DomainModelTypesEnum.Bar }
        };

        private readonly RevitDataExchange.RevitDataExchangeClient _client;
        
        public RevitExtraDataExchangeClient(RevitDataExchange.RevitDataExchangeClient client)
        {
            _client = client;
            var channel = new Channel("127.0.0.1:5005", ChannelCredentials.Insecure);
            _client = new RevitDataExchange.RevitDataExchangeClient(channel);
        }

        public async Task<InstanceData[]> PullDataFromRevitInstancesByType(Type type, DocumentDescriptor documentDescriptor)
        {
            if (!TypesMapping.TryGetValue(type, out var requiredType))
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }
            
            var response = await _client.PullDataFromRevitInstancesByTypeAsync(
                new PullDataFromRevitInstancesByTypeRequest
                {
                    DocumentId = documentDescriptor.Id,
                    Type = requiredType
                });

            if (response?.ErrorInfo.Code != ExceptionCodeEnum.Success)
            {
                throw new InvalidOperationException($"Code: {response?.ErrorInfo.Code} Message: {response?.ErrorInfo.Message}");
            }

            return response.InstancesData.ToArray();
        }
    }
}

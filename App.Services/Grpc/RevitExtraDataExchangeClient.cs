using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public async Task<string> PullDataFromRevitInstance(Type type, DocumentDescriptor documentDescriptor, int instanceId)
        {
            if (!TypesMapping.TryGetValue(type, out var requiredType))
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }

            var response = await _client.PullDataFromRevitInstanceAsync(
                new PullDataFromRevitInstanceRequest
                {
                    InstanceId = instanceId,
                    DocumentId = documentDescriptor.Id,
                    Type = requiredType
                });

            if (response?.ErrorInfo.Code != ExceptionCodeEnum.Success)
            {
                throw new InvalidOperationException($"Code: {response?.ErrorInfo.Code} Message: {response?.ErrorInfo.Message}");
            }

            return response.Data;
        }

        public async Task PushDataToRevitInstance(
            Type type, 
            DocumentDescriptor documentDescriptor,
            InstanceData instanceData)
        {
            if (!TypesMapping.TryGetValue(type, out var requiredType))
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }

            var response = await _client.PushDataToRevitInstanceAsync(
                new PushDataToRevitInstanceRequest
                {
                    DocumentId = documentDescriptor.Id,
                    InstanceData = instanceData,
                    Type = requiredType
                });

            if (response?.ErrorInfo.Code != ExceptionCodeEnum.Success)
            {
                throw new InvalidOperationException($"Code: {response?.ErrorInfo.Code} Message: {response?.ErrorInfo.Message}");
            }
        }

        public async Task<int> CreateRevitElement(Type type, DocumentDescriptor documentDescriptor)
        {
            if (!TypesMapping.TryGetValue(type, out var requiredType))
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }

            var response = await _client.CreateRevitInstanceAsync(
                new CreateRevitInstanceRequest
                {
                    DocumentId = documentDescriptor.Id,
                    Type = requiredType
                });

            if (response?.ErrorInfo.Code != ExceptionCodeEnum.Success)
            {
                throw new InvalidOperationException($"Code: {response?.ErrorInfo.Code} Message: {response?.ErrorInfo.Message}");
            }

            if (response.InstanceId <= 0)
            {
                throw new InvalidOperationException($"Incorrect ID returned after instance creation.");
            }

            return response.InstanceId;
        }

        public async Task<bool> DeleteRevitElement(int instanceId, DocumentDescriptor documentDescriptor)
        {
            var response = await _client.DeleteRevitInstanceAsync(
                new DeleteRevitInsatnceRequest
                {
                    DocumentId = documentDescriptor.Id,
                    InstanceId = instanceId
                });

            return response?.ErrorInfo.Code == ExceptionCodeEnum.Success;
        }
    }
}

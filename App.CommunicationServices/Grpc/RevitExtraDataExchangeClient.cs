using App.DML;
using Grpc.Core;
using Revit.Services.Grpc.Services;

namespace App.CommunicationServices.Grpc
{
    public class RevitExtraDataExchangeClient
    {
        private static readonly Dictionary<Type, DomainModelTypesEnum> TypesMapping = new()
        {
            { typeof(Foo), DomainModelTypesEnum.Foo },
            { typeof(Bar), DomainModelTypesEnum.Bar },
            { typeof(BcLevel), DomainModelTypesEnum.Bar }
        };

        private readonly RevitDataExchange.RevitDataExchangeClient _client;
        
        public RevitExtraDataExchangeClient()
        {
            var channel = new Channel("127.0.0.1:5005", ChannelCredentials.Insecure);
            _client = new RevitDataExchange.RevitDataExchangeClient(channel);
        }

        public async Task<int[]> Allocate(Type type)
        {
            if (!TypesMapping.TryGetValue(type, out var requiredType))
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }

            var response = await _client.AllocateRevitInstancesByTypeAsync(
                new AllocateRevitInstancesByTypeRequest
                {
                    AllocationType = requiredType
                });

            if (response?.ErrorInfo.Code != ExceptionCodeEnum.Success)
            {
                throw new InvalidOperationException($"Code: {response?.ErrorInfo.Code} Message: {response?.ErrorInfo.Message}");
            }

            return response.AllocatedItemsId.ToArray();
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

        public async Task<Level[]> GetLevelsFromRevit(DocumentDescriptor documentDescriptor)
        {
            var response = await _client.GetLevelsFromRevitAsync(
                new GetLevelsFromRevitRequest
                {
                    DocumentId = documentDescriptor.Id
                });
        
            if (response?.ErrorInfo.Code != ExceptionCodeEnum.Success)
            {
                throw new InvalidOperationException($"Code: {response?.ErrorInfo.Code} Message: {response?.ErrorInfo.Message}");
            }
        
            return response.Levels.ToArray();
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

        public async Task DeleteRevitElement(int instanceId, DocumentDescriptor documentDescriptor)
        {
            var response = await _client.DeleteRevitInstanceAsync(
                new DeleteRevitInsatnceRequest
                {
                    DocumentId = documentDescriptor.Id,
                    InstanceId = instanceId
                });

            if (response.ErrorInfo.Code != ExceptionCodeEnum.Success)
            {
                throw new InvalidOperationException($"Instance with provided ID={instanceId} wasn't found.");
            }
        }

        public async Task CreateOrUpdateCatalogRecordAsync(
            DocumentDescriptor documentDescriptor, 
            CatalogRecordData catalogRecordData)
        {
            var response = await _client.CreateOrUpdateCatalogRecordAsync(
                new CreateOrUpdateRecordInCatalogRequest
                {
                    CatalogRecordData = catalogRecordData,
                    DocumentId = documentDescriptor.Id,
                });

            if (response.ErrorInfo.Code != ExceptionCodeEnum.Success)
            {
                throw new InvalidOperationException($"Code: {response.ErrorInfo.Code} " +
                                                    $"Message: {response.ErrorInfo.Message}");
            }
        }

        public async Task<CatalogRecordData> ReadRecordFromCatalogAsync(
            DocumentDescriptor documentDescriptor,
            Guid uniqueId)
        {
            var response = await _client.ReadRecordFromCatalogAsync(
                new ReadRecordFromCatalogRequest
                {
                    GuidId = uniqueId.ToString(),
                    DocumentId = documentDescriptor.Id
                });

            if (response.ErrorInfo.Code == ExceptionCodeEnum.CatalogRecordWasNotFound)
            {
                return null;
            }

            if (response.ErrorInfo.Code != ExceptionCodeEnum.Success)
            {
                throw new InvalidOperationException($"Code: {response.ErrorInfo.Code} " +
                                                    $"Message: {response.ErrorInfo.Message}");
            }

            return response.CatalogRecordData;
        }

        //todo segregate interfaces
        public async Task<IEnumerable<CatalogRecordData>> ReadAllRecordsFromCatalogAsync(
            DocumentDescriptor documentDescriptor)
        {
            var response = await _client.ReadAllRecordsFromCatalogAsync(
                new ReadAllRecordsFromCatalogRequest
                {
                    DocumentId = documentDescriptor.Id
                });

            if (response.ErrorInfo.Code != ExceptionCodeEnum.Success)
            {
                throw new InvalidOperationException($"Code: {response.ErrorInfo.Code} " +
                                                    $"Message: {response.ErrorInfo.Message}");
            }

            return response.Records;
        }
    }
}

using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using App.Catalog.Db.Model;
using App.CommunicationServices.Grpc;
using Revit.Services.Grpc.Services;

namespace App.DAL.Common.Services.Catalog
{
    public class RevitCatalogStorage
    {
        private readonly RevitExtraDataExchangeClient _client;

        private readonly DocumentDescriptor _documentDescriptor;
        
        private readonly JsonSerializerOptions _serializerOptions;

        public RevitCatalogStorage(RevitExtraDataExchangeClient client, DocumentDescriptor documentDescriptor)
        {
            _client = client;
            _documentDescriptor = documentDescriptor;
            _serializerOptions = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
        }

        public async Task<T> ReadCatalogRecordOrDefaultAsync<T>(Guid uniqueId) where T : BaseCatalogEntity
        {
            var readRecord = await _client.ReadRecordFromCatalogAsync(_documentDescriptor, uniqueId);
            
            return readRecord is null ? null : JsonSerializer.Deserialize<T>(readRecord.Data);
        }

        public async Task<BaseCatalogEntity> ReadCatalogRecordOrDefaultAsync(Guid uniqueId, Type recordType)
        {
            var readRecord = await _client.ReadRecordFromCatalogAsync(_documentDescriptor, uniqueId);

            return readRecord is null
                ? null 
                : (BaseCatalogEntity)JsonSerializer.Deserialize(readRecord.Data, recordType);
        }

        private async Task SendDataToRevit(string serializedRecord, string uniqueId)
        {
            await _client.CreateOrUpdateCatalogRecordAsync(_documentDescriptor,
                new CatalogRecordData
                {
                    Data = serializedRecord,
                    GuidId = uniqueId
                });
        }

        public async Task WriteCatalogRecordAsync(BaseCatalogEntity record, Type recordType)
        {
            var serializedRecord = JsonSerializer.Serialize(record, recordType, _serializerOptions);

            await SendDataToRevit(serializedRecord, record.IdGuid.ToString());
        }

        public async Task WriteCatalogRecordAsync<T>(T record) where T : BaseCatalogEntity
        {
            var serializedRecord = JsonSerializer.Serialize(record, _serializerOptions);

            await SendDataToRevit(serializedRecord, record.IdGuid.ToString());
        }

        public async Task<IEnumerable<BaseCatalogEntity>> ReadAllCatalogRecordsAsync()
        {
            var readRecords = 
                (await _client.ReadAllRecordsFromCatalogAsync(_documentDescriptor)).ToArray();

            return readRecords.Any() 
                ? readRecords.Select(x => JsonSerializer.Deserialize<BaseCatalogEntity>(x.Data))
                : Enumerable.Empty<BaseCatalogEntity>();
        }
    }
}

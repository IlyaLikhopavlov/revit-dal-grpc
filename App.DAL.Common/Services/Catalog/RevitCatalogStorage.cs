using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using App.Catalog.Db.Model;
using App.CommunicationServices.Grpc;
using Revit.Services.Grpc.Services;

namespace App.DAL.Common.Services.Catalog
{
    public class RevitCatalogStorage : ICatalogStorage
    {
        private readonly RevitExtraDataExchangeClient _client;

        private readonly DocumentDescriptor _documentDescriptor;
        
        public RevitCatalogStorage(RevitExtraDataExchangeClient client, DocumentDescriptor documentDescriptor)
        {
            _client = client;
            _documentDescriptor = documentDescriptor;
        }

        public async Task<T> ReadCatalogRecord<T>(Guid uniqueId) where T : BaseCatalogEntity
        {
            var readRecord = await _client.ReadRecordFromCatalogAsync(_documentDescriptor, uniqueId);

            return JsonSerializer.Deserialize<T>(readRecord.Data);
        }


        public async Task WriteCatalogRecord<T>(T record) where T : BaseCatalogEntity
        {
            var serializedRecord = JsonSerializer.Serialize(record);

            await _client.CreateOrUpdateCatalogRecordAsync(_documentDescriptor,
                new CatalogRecordData
                {
                    Data = serializedRecord,
                    GuidId = record.IdGuid.ToString()
                });
        }
    }
}

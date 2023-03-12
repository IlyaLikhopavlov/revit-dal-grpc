﻿using Revit.Services.ExternalEvents.Infrastructure;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Revit.ScopedServicesFunctionality;
using Revit.Storage.InstancesAccess;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class ReadRecordFromCatalogEventHandler : ExternalServiceEventHandler<ReadRecordFromCatalogRequest, ReadRecordFromCatalogResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public ReadRecordFromCatalogEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override ReadRecordFromCatalogResponse Execute(Document document)
        {
            var result = 
                new ReadRecordFromCatalogResponse
                {
                    ErrorInfo = new ErrorInfo()
                };

            try
            {
                var revitCatalogContext =
                    _scopeFactory.GetScopedService<IRevitCatalogContext>(document);

                if (revitCatalogContext.Contains(Request.GuidId))
                {
                    result.ErrorInfo.Code = ExceptionCodeEnum.CatalogRecordWasNotFound;
                }

                result.CatalogRecordData = revitCatalogContext.ReadById(Request.GuidId);
            }
            catch (Exception ex)
            {
                result.ErrorInfo.Code = ExceptionCodeEnum.Unknown;
                result.ErrorInfo.Message = ex.Message;

                return result;
            }

            result.ErrorInfo.Code = ExceptionCodeEnum.Success;
            return result;
        }
    }
}

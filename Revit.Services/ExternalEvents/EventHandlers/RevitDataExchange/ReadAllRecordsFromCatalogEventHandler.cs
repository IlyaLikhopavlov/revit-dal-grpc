using Revit.Services.ExternalEvents.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Google.Protobuf.Collections;
using Revit.Services.Grpc.Services;
using Revit.ScopedServicesFunctionality;
using Revit.Storage.InstancesAccess;
using Grpc.Core;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class ReadAllRecordsFromCatalogEventHandler : ExternalServiceEventHandler<ReadAllRecordsFromCatalogRequest, ReadAllRecordsFromCatalogResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public ReadAllRecordsFromCatalogEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override ReadAllRecordsFromCatalogResponse Execute(Document document)
        {
            var result =
                new ReadAllRecordsFromCatalogResponse
                {
                    ErrorInfo = new ErrorInfo()
                };

            try
            {
                var revitCatalogContext =
                    _scopeFactory.GetScopedService<IRevitCatalogContext>(document);
                
                result.Records.AddRange(revitCatalogContext.ReadAll());
                result.ErrorInfo.Code = ExceptionCodeEnum.Success;
            }
            catch (Exception ex)
            {
                result.ErrorInfo.Code = ExceptionCodeEnum.Unknown;
                result.ErrorInfo.Message = ex.Message;

                return result;
            }
            
            return result;
        }
    }
}

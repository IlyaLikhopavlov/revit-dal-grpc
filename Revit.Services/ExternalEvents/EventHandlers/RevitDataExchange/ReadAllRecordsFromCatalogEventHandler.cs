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
            try
            {
                var revitCatalogContext =
                    _scopeFactory.GetScopedService<IRevitCatalogContext>(document);
                
                var result = 
                    new ReadAllRecordsFromCatalogResponse
                    {
                        ErrorInfo = new ErrorInfo
                        {
                            Code = ExceptionCodeEnum.Success
                        }
                    };

                result.Records.AddRange(revitCatalogContext.ReadAll());
                return result;
            }
            catch (Exception ex)
            {
                return new ReadAllRecordsFromCatalogResponse
                {
                    ErrorInfo = new ErrorInfo
                    {
                        Code = ExceptionCodeEnum.Unknown,
                        Message = ex.Message
                    }
                };
            }
        }
    }
}

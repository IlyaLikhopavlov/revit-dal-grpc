using Revit.Services.ExternalEvents.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            CatalogRecordData result;
            try
            {
                var revitDataContext =
                    _scopeFactory?.GetScopedService<IRevitCatalogContext>(document);

                result = revitDataContext?.ReadFromCatalog(Request.GuidId);
            }
            catch (Exception ex)
            {
                return new ReadRecordFromCatalogResponse
                {
                    ErrorInfo =
                        new ErrorInfo
                        {
                            Code = ExceptionCodeEnum.Unknown,
                            Message = ex.Message
                        }
                };
            }

            if (result is null)
            {
                return new ReadRecordFromCatalogResponse
                {
                    ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Unknown }
                };
            }

            return new ReadRecordFromCatalogResponse
            {
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success },
                CatalogRecordData = result
            };
        }
    }
}

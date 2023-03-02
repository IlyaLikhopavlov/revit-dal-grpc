using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Revit.ScopedServicesFunctionality;
using Revit.Services.ExternalEvents.Infrastructure;
using Revit.Services.Grpc.Services;
using Revit.Storage.InstancesAccess;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class CreateOrUpdateCatalogRecordEventHandler : ExternalServiceEventHandler<CreateOrUpdateRecordInCatalogRequest, BasicResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public CreateOrUpdateCatalogRecordEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override BasicResponse Execute(Document document)
        {
            var result = new BasicResponse();
            try
            {
                var catalogContext =
                    _scopeFactory.GetScopedService<IRevitCatalogContext>(document);

                catalogContext.CreateOrUpdateRecordInCatalog(Request.CatalogRecordData);
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

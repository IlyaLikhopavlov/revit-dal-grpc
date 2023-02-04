using Revit.Services.ExternalEvents.Infrastructure;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.ScopedServicesFunctionality;
using Microsoft.Extensions.DependencyInjection;
using Revit.Storage.InstancesAccess;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class PushDataToRevitInstancesEventHandler : ExternalServiceEventHandler<PushDataToRevitInstancesRequest, BasicResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public PushDataToRevitInstancesEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override BasicResponse Execute(Document document)
        {
            try
            {
                var documentScope = _scopeFactory?.CreateScope(document);
                var revitDataContext = documentScope?
                    .ServiceProvider
                    .GetService<IFactory<Document, RevitDataContext>>()?
                    .New(document);

                revitDataContext?.PushDataIntoInstancesById(Request.InstanceData, Request.Type);
            }
            catch (Exception ex)
            {
                return new BasicResponse
                {
                    ErrorInfo =
                        new ErrorInfo
                        {
                            Code = ExceptionCodeEnum.Unknown,
                            Message = ex.Message
                        }
                };
            }

            return new BasicResponse
            {
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success },
            };
        }
    }
}
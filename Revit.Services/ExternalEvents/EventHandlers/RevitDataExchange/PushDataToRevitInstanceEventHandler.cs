using Revit.Services.ExternalEvents.Infrastructure;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.ScopedServicesFunctionality;
using Revit.Storage.InstancesAccess;
using Microsoft.Extensions.DependencyInjection;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class
        PushDataToRevitInstanceEventHandler : ExternalServiceEventHandler<PushDataToRevitInstanceRequest, BasicResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public PushDataToRevitInstanceEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override BasicResponse Execute(Document document)
        {
            try
            {
                var revitDataContext =
                    _scopeFactory.GetScopedService<RevitDataContext>(document);
                    
                revitDataContext?.PushDataToRevitInstance(Request.InstanceData, Request.Type);
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
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success }
            };
        }
    }
}

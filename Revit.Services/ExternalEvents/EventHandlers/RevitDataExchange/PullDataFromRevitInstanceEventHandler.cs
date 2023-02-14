using Autodesk.Revit.DB;
using Revit.ScopedServicesFunctionality;
using Revit.Services.ExternalEvents.Infrastructure;
using Revit.Services.Grpc.Services;
using Revit.Storage.InstancesAccess;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class PullDataFromRevitInstanceEventHandler : ExternalServiceEventHandler<PullDataFromRevitInstanceRequest, PullDataFromRevitInstanceResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public PullDataFromRevitInstanceEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override PullDataFromRevitInstanceResponse Execute(Document document)
        {

            InstanceData result;
            try
            {
                var revitDataContext =
                    _scopeFactory?.GetScopedService<RevitDataContext>(document);

                result = revitDataContext?.PullDataFromRevitInstance(Request.InstanceId,  Request.Type);
            }
            catch (Exception ex)
            {
                return new PullDataFromRevitInstanceResponse
                {
                    ErrorInfo =
                        new ErrorInfo
                        {
                            Code = ExceptionCodeEnum.Unknown,
                            Message = ex.Message
                        }
                };
            }

            var response = new PullDataFromRevitInstanceResponse
            {
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success },
                Data = result?.Data
            };

            return response;
        }
    }
}

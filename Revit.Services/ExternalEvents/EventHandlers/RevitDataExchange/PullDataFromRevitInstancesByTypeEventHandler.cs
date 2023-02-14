using Revit.Services.ExternalEvents.Infrastructure;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Revit.ScopedServicesFunctionality;
using Revit.Storage.InstancesAccess;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class PullDataFromRevitInstancesByTypeEventHandler 
        : ExternalServiceEventHandler<PullDataFromRevitInstancesByTypeRequest, PullDataFromRevitInstancesResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public PullDataFromRevitInstancesByTypeEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override PullDataFromRevitInstancesResponse Execute(Document document)
        {

            InstanceData[] result;
            try
            {
                var revitDataContext =
                    _scopeFactory?.GetScopedService<RevitDataContext>(document);

                result = revitDataContext?.PullDataFromInstancesByType(Request.Type).ToArray();
            }
            catch (Exception ex)
            {
                return new PullDataFromRevitInstancesResponse
                {
                    ErrorInfo =
                        new ErrorInfo
                        {
                            Code = ExceptionCodeEnum.Unknown,
                            Message = ex.Message
                        }
                };
            }

            var response = new PullDataFromRevitInstancesResponse
            {
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success },
            };
            response.InstancesData.Add(result);

            return response;
        }
    }
}

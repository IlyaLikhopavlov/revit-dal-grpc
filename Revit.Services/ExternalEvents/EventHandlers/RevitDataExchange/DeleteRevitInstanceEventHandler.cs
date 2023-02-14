using Revit.ScopedServicesFunctionality;
using Revit.Services.ExternalEvents.Infrastructure;
using Revit.Services.Grpc.Services;
using Revit.Storage.InstancesAccess;
using Autodesk.Revit.DB;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class DeleteRevitInstanceEventHandler : ExternalServiceEventHandler<DeleteRevitInsatnceRequest, BasicResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public DeleteRevitInstanceEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override BasicResponse Execute(Document document)
        {
            bool? result;
            try
            {
                var revitDataContext = _scopeFactory?.GetScopedService<RevitDataContext>(document);

                result = revitDataContext?.DeleteRevitElement(Request.InstanceId);
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

            if (result is null or false)
            {
                return new BasicResponse
                {
                    ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Unknown }
                };
            }

            return new BasicResponse
            {
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success }
            };
        }
    }
}

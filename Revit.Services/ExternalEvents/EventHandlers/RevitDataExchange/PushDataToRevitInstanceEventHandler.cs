using Revit.Services.ExternalEvents.Infrastructure;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    internal class PushDataToRevitInstanceEventHandler : ExternalServiceEventHandler<PushDataToRevitInstanceRequest, BasicResponse>
    {
        public PushDataToRevitInstanceEventHandler()
        {
            
        }


        protected override BasicResponse Execute(Document document)
        {

            return 
                new BasicResponse 
                { 
                    ErrorInfo = new ErrorInfo
                    {
                        Code = ExceptionCodeEnum.Success
                    }
                };
        }
    }
}

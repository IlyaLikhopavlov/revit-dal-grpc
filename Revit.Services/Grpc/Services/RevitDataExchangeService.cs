using Grpc.Core;

namespace Revit.Services.Grpc.Services
{
    public class RevitDataExchangeService : RevitDataExchange.RevitDataExchangeBase
    {
        public override Task<BasicResponse> PushDataToRevitInstance(PushDataToRevitInstanceRequest request, ServerCallContext context)
        {
            return base.PushDataToRevitInstance(request, context);
        }

        public override Task<PullDataFromRevitInstanceResponse> PullDataFromRevitInstance(PullDataFromRevitInstanceRequest request, ServerCallContext context)
        {
            return base.PullDataFromRevitInstance(request, context);
        }
    }
}

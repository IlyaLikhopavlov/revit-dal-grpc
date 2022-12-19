using Grpc.Core;

namespace Revit.Services.Grpc.Services
{
    public class RevitDataExchangeService : RevitDataExchange.RevitDataExchangeBase
    {
        public override Task<PullDataFromRevitInstancesResponse> PullDataFromRevitInstances(PullDataFromRevitInstancesRequest request, ServerCallContext context)
        {
            return base.PullDataFromRevitInstances(request, context);
        }

        public override Task<PullDataFromRevitInstancesResponse> PullDataFromRevitInstancesByType(PullDataFromRevitInstancesByTypeRequest request, ServerCallContext context)
        {
            return base.PullDataFromRevitInstancesByType(request, context);
        }

        public override Task<AllocateRevitInstancesByTypeResponse> AllocateRevitInstancesByType(AllocateRevitInstancesByTypeRequest request, ServerCallContext context)
        {
            return base.AllocateRevitInstancesByType(request, context);
        }

        public override Task<BasicResponse> PushDataToRevitInstance(PushDataToRevitInstanceRequest request, ServerCallContext context)
        {
            return base.PushDataToRevitInstance(request, context);
        }

        public override Task<BasicResponse> PushDataToRevitInstances(PushDataToRevitInstancesRequest request, ServerCallContext context)
        {
            return base.PushDataToRevitInstances(request, context);
        }

        public override Task<PullDataFromRevitInstanceResponse> PullDataFromRevitInstance(PullDataFromRevitInstanceRequest request, ServerCallContext context)
        {
            return base.PullDataFromRevitInstance(request, context);
        }


    }
}

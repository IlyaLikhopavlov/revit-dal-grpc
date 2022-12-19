using Grpc.Core;
using Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange;
using Revit.Services.ExternalEvents.Infrastructure;

namespace Revit.Services.Grpc.Services
{
    public class RevitDataExchangeService : RevitDataExchange.RevitDataExchangeBase
    {
        private readonly IExternalEventsService _externalEventsService;

        public RevitDataExchangeService(IExternalEventsService externalEventsService)
        {
            _externalEventsService = externalEventsService;
        }

        public override Task<PullDataFromRevitInstancesResponse> PullDataFromRevitInstances(PullDataFromRevitInstancesRequest request, ServerCallContext context)
        {
            return base.PullDataFromRevitInstances(request, context);
        }

        public override Task<PullDataFromRevitInstancesResponse> PullDataFromRevitInstancesByType(
            PullDataFromRevitInstancesByTypeRequest request, ServerCallContext context) =>
            _externalEventsService.Execute<PullDataFromRevitInstancesByTypeRequest, PullDataFromRevitInstancesResponse>(
                nameof(PullDataFromRevitInstancesByTypeEventHandler), request);
        

        public override Task<AllocateRevitInstancesByTypeResponse> AllocateRevitInstancesByType(
            AllocateRevitInstancesByTypeRequest request, ServerCallContext context) =>
            _externalEventsService.Execute<AllocateRevitInstancesByTypeRequest, AllocateRevitInstancesByTypeResponse>(
                nameof(AllocateRevitInstancesByTypeEventHandler), request);

        public override Task<BasicResponse> PushDataToRevitInstance(PushDataToRevitInstanceRequest request, ServerCallContext context)
        {
            return base.PushDataToRevitInstance(request, context);
        }

        public override Task<BasicResponse> PushDataToRevitInstances(
            PushDataToRevitInstancesRequest request, ServerCallContext context) =>
            _externalEventsService.Execute<PushDataToRevitInstancesRequest, BasicResponse>(
                nameof(PushDataToRevitInstancesEventHandler), request);

        public override Task<PullDataFromRevitInstanceResponse> PullDataFromRevitInstance(PullDataFromRevitInstanceRequest request, ServerCallContext context)
        {
            return base.PullDataFromRevitInstance(request, context);
        }


    }
}

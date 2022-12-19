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

        public override async Task<PullDataFromRevitInstancesResponse> PullDataFromRevitInstancesByType(
            PullDataFromRevitInstancesByTypeRequest request, ServerCallContext context) =>
            await _externalEventsService.Execute<PullDataFromRevitInstancesByTypeRequest, PullDataFromRevitInstancesResponse>(
                nameof(PullDataFromRevitInstancesByTypeEventHandler), request);

        public override async Task<AllocateRevitInstancesByTypeResponse> AllocateRevitInstancesByType(
            AllocateRevitInstancesByTypeRequest request, ServerCallContext context) =>
            await _externalEventsService.Execute<AllocateRevitInstancesByTypeRequest, AllocateRevitInstancesByTypeResponse>(
                nameof(AllocateRevitInstancesByTypeEventHandler), request);

        public override async Task<BasicResponse> PushDataToRevitInstances(
            PushDataToRevitInstancesRequest request, ServerCallContext context) =>
            await _externalEventsService.Execute<PushDataToRevitInstancesRequest, BasicResponse>(
                nameof(PushDataToRevitInstancesEventHandler), request);

        public override async Task<BasicResponse> PushDataToRevitInstance(
            PushDataToRevitInstanceRequest request, ServerCallContext context) =>
            await _externalEventsService.Execute<PushDataToRevitInstanceRequest, BasicResponse>(
                nameof(PushDataToRevitInstanceEventHandler), request);

        public override async Task<PullDataFromRevitInstanceResponse> PullDataFromRevitInstance(
            PullDataFromRevitInstanceRequest request, ServerCallContext context) =>
            await _externalEventsService.Execute<PullDataFromRevitInstanceRequest, PullDataFromRevitInstanceResponse>(
                nameof(PullDataFromRevitInstanceEventHandler), request);
    }
}

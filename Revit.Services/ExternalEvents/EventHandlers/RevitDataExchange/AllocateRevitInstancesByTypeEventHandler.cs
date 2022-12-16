using Revit.Services.ExternalEvents.Infrastructure;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.ScopedServicesFunctionality;
using Revit.Services.Allocation;
using Microsoft.Extensions.DependencyInjection;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    internal class AllocateRevitInstancesByTypeEventHandler : ExternalServiceEventHandler<AllocateRevitInstancesByTypeRequest, AllocateRevitInstancesByTypeResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public AllocateRevitInstancesByTypeEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override AllocateRevitInstancesByTypeResponse Execute(Document document)
        {
            var documentScope = _scopeFactory?.CreateScope(document);
            var allocationService = documentScope?
                .ServiceProvider
                .GetService<IFactory<Document, ModelItemsAllocationService>>()
                ?.New(document);

            allocationService?.AllocateBar();
        }
    }
}

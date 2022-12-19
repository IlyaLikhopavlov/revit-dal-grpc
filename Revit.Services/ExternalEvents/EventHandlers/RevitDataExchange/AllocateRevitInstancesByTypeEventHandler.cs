using Revit.Services.ExternalEvents.Infrastructure;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.ScopedServicesFunctionality;
using Revit.Services.Allocation;
using Microsoft.Extensions.DependencyInjection;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class AllocateRevitInstancesByTypeEventHandler : ExternalServiceEventHandler<AllocateRevitInstancesByTypeRequest, AllocateRevitInstancesByTypeResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public AllocateRevitInstancesByTypeEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override AllocateRevitInstancesByTypeResponse Execute(Document document)
        {
            int[] itemsId;
            try
            {
                var documentScope = _scopeFactory?.CreateScope(document);
                var allocationService = documentScope?
                    .ServiceProvider
                    .GetService<IFactory<Document, ModelItemsAllocationService>>()
                    ?.New(document);

                itemsId = allocationService?.AllocateInstance(Request.AllocationType);
            }
            catch (Exception ex)
            {
                return new AllocateRevitInstancesByTypeResponse
                {
                    ErrorInfo =
                        new ErrorInfo
                        {
                            Code = ExceptionCodeEnum.Unknown,
                            Message = ex.Message
                        }
                };
            }

            if (itemsId?.Length == 0)
            {
                return new AllocateRevitInstancesByTypeResponse
                {
                    ErrorInfo = 
                        new ErrorInfo
                        {
                            Code = ExceptionCodeEnum.Unknown,
                            Message = @"No items are allocated"
                        }
                };
            }

            return new AllocateRevitInstancesByTypeResponse
            {
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success },
                AllocatedItemsId = { itemsId }
            };
        }
    }
}

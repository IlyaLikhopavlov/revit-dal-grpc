using Revit.Services.ExternalEvents.Infrastructure;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Revit.ScopedServicesFunctionality;
using Revit.Storage.InstancesAccess;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class ReadRecordFromCatalogEventHandler : ExternalServiceEventHandler<ReadRecordFromCatalogRequest, ReadRecordFromCatalogResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public ReadRecordFromCatalogEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override ReadRecordFromCatalogResponse Execute(Document document)
        {
            try
            {
                var revitCatalogContext =
                    _scopeFactory.GetScopedService<IRevitCatalogContext>(document);

                if (!revitCatalogContext.Contains(Request.GuidId))
                {
                    return new ReadRecordFromCatalogResponse
                    {
                        ErrorInfo = new ErrorInfo
                        {
                            Code = ExceptionCodeEnum.CatalogRecordWasNotFound
                        }
                    };
                }

                return new ReadRecordFromCatalogResponse
                {
                    CatalogRecordData = revitCatalogContext.ReadById(Request.GuidId),
                    ErrorInfo = new ErrorInfo
                    {
                        Code = ExceptionCodeEnum.Success
                    }
                };
                    
            }
            catch (Exception ex)
            {
                return new ReadRecordFromCatalogResponse
                {
                    ErrorInfo = new ErrorInfo
                    {
                        Code = ExceptionCodeEnum.Unknown,
                        Message = ex.Message
                    }
                };
            }
        }
    }
}

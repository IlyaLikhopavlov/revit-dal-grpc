using Revit.Services.ExternalEvents.Infrastructure;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Revit.ScopedServicesFunctionality;
using Revit.Storage.InstancesAccess;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange;

public class GetLevelsFromRevitEventHandler
    : ExternalServiceEventHandler<GetLevelsFromRevitRequest, GetLevelsFromRevitResponse>
{
    private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

    public GetLevelsFromRevitEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override GetLevelsFromRevitResponse Execute(Document document)
    {
        try
        {
            var revitDataContext =
                _scopeFactory?.GetScopedService<RevitDataContext>(document);

            return new GetLevelsFromRevitResponse
            {
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success },
                Levels = { revitDataContext?.GetLevelsFromRevit() }
            };
        }
        catch (Exception ex)
        {
            return new GetLevelsFromRevitResponse
            {
                ErrorInfo =
                    new ErrorInfo
                    {
                        Code = ExceptionCodeEnum.Unknown,
                        Message = ex.Message
                    }
            };
        }
    }
}
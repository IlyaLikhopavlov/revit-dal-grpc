﻿using Revit.Services.ExternalEvents.Infrastructure;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Revit.ScopedServicesFunctionality;
using Revit.Storage.InstancesAccess;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class PushDataToRevitInstancesEventHandler : ExternalServiceEventHandler<PushDataToRevitInstancesRequest, BasicResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public PushDataToRevitInstancesEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override BasicResponse Execute(Document document)
        {
            try
            {
                var revitDataContext =
                    _scopeFactory?.GetScopedService<RevitDataContext>(document);

                revitDataContext?.PushDataIntoInstancesById(Request.InstanceData, Request.Type);
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

            return new BasicResponse
            {
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success },
            };
        }
    }
}
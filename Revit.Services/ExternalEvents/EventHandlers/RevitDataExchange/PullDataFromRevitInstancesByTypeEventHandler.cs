using Revit.Services.ExternalEvents.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Revit.ScopedServicesFunctionality;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Google.Protobuf.Collections;
using Microsoft.Extensions.DependencyInjection;
using Revit.Storage.InstancesAccess;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class PullDataFromRevitInstancesByTypeEventHandler 
        : ExternalServiceEventHandler<PullDataFromRevitInstancesByTypeRequest, PullDataFromRevitInstancesResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public PullDataFromRevitInstancesByTypeEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override PullDataFromRevitInstancesResponse Execute(Document document)
        {

            InstanceData[] result;
            try
            {
                var documentScope = _scopeFactory?.CreateScope(document);
                var revitDataContext = documentScope?
                    .ServiceProvider
                    .GetService<IFactory<Document, RevitDataContext>>()?
                    .New(document);

                result = revitDataContext?.PullDataFromInstancesByType(Request.Type).ToArray();
            }
            catch (Exception ex)
            {
                return new PullDataFromRevitInstancesResponse
                {
                    ErrorInfo =
                        new ErrorInfo
                        {
                            Code = ExceptionCodeEnum.Unknown,
                            Message = ex.Message
                        }
                };
            }

            var response = new PullDataFromRevitInstancesResponse
            {
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success },
            };
            response.InstancesData.Add(result);

            return response;
        }
    }
}

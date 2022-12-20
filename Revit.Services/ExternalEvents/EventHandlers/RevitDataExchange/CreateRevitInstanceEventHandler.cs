using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Revit.ScopedServicesFunctionality;
using Revit.Services.ExternalEvents.Infrastructure;
using Revit.Storage.InstancesAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Revit.Services.Grpc.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange
{
    public class CreateRevitInstanceEventHandler : ExternalServiceEventHandler<CreateRevitInstanceRequest, CreateRevitInstanceResponse>
    {
        private readonly IRevitDocumentServiceScopeFactory _scopeFactory;

        public CreateRevitInstanceEventHandler(IRevitDocumentServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override CreateRevitInstanceResponse Execute(Document document)
        {

            int? result;
            try
            {
                var documentScope = _scopeFactory?.CreateScope(document);
                var revitDataContext = documentScope?
                    .ServiceProvider
                    .GetService<IFactory<Document, RevitDataContext>>()?
                    .New(document);

                result = revitDataContext?.CreateRevitElement(Request.Type);
            }
            catch (Exception ex)
            {
                return new CreateRevitInstanceResponse
                {
                    ErrorInfo =
                        new ErrorInfo
                        {
                            Code = ExceptionCodeEnum.Unknown,
                            Message = ex.Message
                        }
                };
            }

            if (result is null)
            {
                return new CreateRevitInstanceResponse
                {
                    ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Unknown }
                };
            }

            return new CreateRevitInstanceResponse
            {
                ErrorInfo = new ErrorInfo { Code = ExceptionCodeEnum.Success },
                InstanceId = result.Value
            };
        }
    }
}

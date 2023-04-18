using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revit.ScopedServicesFunctionality;
using Revit.Storage.InstancesAccess;
using Microsoft.Extensions.DependencyInjection;
using Revit.Services.ExternalEvents.EventHandlers.RevitDataExchange;
using Revit.Services.Processing;

namespace Revit.AddIn.Commands
{
    [Transaction(TransactionMode.Manual)]
    public class GetLevelsCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            // UIApplication ui;
           
            // using var scopeFactory = RevitDalApp.ServiceProvider.GetService<IRevitDocumentServiceScopeFactory>();
            // using var scopeUiApplication = RevitDalApp.ServiceProvider.GetService<ApplicationProcessing>();
            //
            // var pullDataFromRevitHandler = new PullDataFromRevitInstancesByTypeEventHandler(scopeFactory);
            //
            // pullDataFromRevitHandler.Execute(scopeUiApplication?.UiApplication);

            return Result.Succeeded;
        }
    }
}
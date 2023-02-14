using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Microsoft.Extensions.DependencyInjection;
using Revit.ScopedServicesFunctionality;
using Revit.Services.Allocation;

namespace Revit.AddIn.Commands
{
    [Transaction(TransactionMode.Manual)]
    internal class PlaceBarCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Result.Succeeded;
        }
    }
}

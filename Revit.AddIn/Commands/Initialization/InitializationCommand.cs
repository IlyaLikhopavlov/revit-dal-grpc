using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Microsoft.Extensions.DependencyInjection;
using Revit.Services.Processing;

namespace Revit.AddIn.Commands.Initialization
{
    public class InitializationCommand : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Execute(commandData.Application);
        }

        public Result Execute(UIApplication uiApplication)
        {
            try
            {
                var applicationProcessing = RevitDalApp.ServiceProvider.GetService<ApplicationProcessing>();
                applicationProcessing!.UiApplication = uiApplication;

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                TaskDialog.Show("Error", $"{ex}");
                return Result.Failed;
            }
        }
    }
}

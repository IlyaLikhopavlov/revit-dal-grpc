using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace Revit.AddIn.Commands
{
    public class InitializationCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Execute(commandData.Application);
        }

        public Result Execute(UIApplication uiApplication)
        {
            try
            {
                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                return Result.Failed;
            }
        }
    }
}

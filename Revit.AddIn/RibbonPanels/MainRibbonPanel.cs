using System.Reflection;
using Autodesk.Revit.UI;
using Revit.AddIn.Commands;

namespace Revit.AddIn.RibbonPanels
{
    public class MainRibbonPanel
    {
        private const string RevitAndMdiTabName = @"Revit DAL";

        private const string ToolButtonsPanelName = @"Tools Button";

        public void Create(UIControlledApplication application)
        {
            application.CreateRibbonTab(RevitAndMdiTabName);

            CreateToolButtonsPanel(application, RevitAndMdiTabName);
        }

        private static void CreateToolButtonsPanel(UIControlledApplication application, string tabName)
        {
            var revitPanel = application.CreateRibbonPanel(tabName, ToolButtonsPanelName);

            var path = Assembly.GetExecutingAssembly().Location;

            var pushButtonCommon =
                new PushButtonData(@"DoWork", @"Do Work", path, typeof(DoSomeWorkCommand).FullName);

            revitPanel.AddItem(pushButtonCommon);
        }
    }
}

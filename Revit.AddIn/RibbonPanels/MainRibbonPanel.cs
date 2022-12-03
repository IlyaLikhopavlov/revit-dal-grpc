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

            var fooButtonCommon =
                new PushButtonData(@"Foo", @"Foo", path, typeof(PlaceFooCommand).FullName);

            var barButtonCommand =
                new PushButtonData(@"Bar", @"Bar", path, typeof(PlaceBarCommand).FullName);

            var readDataCommand =
                new PushButtonData(@"Read", @"Read", path, typeof(ReadDataCommand).FullName);

            revitPanel.AddItem(fooButtonCommon);
            revitPanel.AddItem(barButtonCommand);
            revitPanel.AddItem(readDataCommand);
        }
    }
}

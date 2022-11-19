using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.UI;

namespace Revit.DAL.Processing
{
    public class ApplicationProcessing
    {

        public ApplicationProcessing(UIControlledApplication application)
        {
            UiControlledApplication = application;
            ControlledApplication = application?.ControlledApplication;
        }

        private UIApplication _uiApplication;

        public UIApplication UiApplication
        {
            get
            {
                if (_uiApplication == null)
                {
                    throw new InvalidOperationException(@"UIApplication must be initialized earlier.");
                }

                return _uiApplication;
            }
            set
            {
                if (_uiApplication != null)
                {
                    throw new InvalidOperationException(@"UIApplication has been set already.");
                }

                _uiApplication = value;
            }
        }

        public UIControlledApplication UiControlledApplication { get; }

        public ControlledApplication ControlledApplication { get; set; }
    }
}

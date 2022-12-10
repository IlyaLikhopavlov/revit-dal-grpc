using Autodesk.Revit.DB;

namespace Revit.Services.Processing.EventArgs
{
    public class DocumentChangedEventArgs : System.EventArgs
    {
        public Document ActiveDocument { get; set; }
    }
}

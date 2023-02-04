using Autodesk.Revit.DB;


namespace Revit.Services.Processing.EventArgs
{
    public class DocumentClosingEventArgs : System.EventArgs
    {
        public Document ClosingDocument { get; set; }
    }
}

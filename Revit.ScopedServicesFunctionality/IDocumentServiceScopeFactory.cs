using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;

namespace Revit.ScopedServicesFunctionality
{
    public interface IRevitDocumentServiceScopeFactory : IDocumentServiceScopeFactory<Document>, IDisposable
    {
        //EventHandler<DocumentClosingEventArgs> DocumentClosing { get; set; }
    }
}

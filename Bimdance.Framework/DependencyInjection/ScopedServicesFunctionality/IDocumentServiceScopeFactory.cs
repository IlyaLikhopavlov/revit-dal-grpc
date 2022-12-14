using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality
{
    public interface IDocumentServiceScopeFactory : IDisposable
    {
        IServiceScope CreateScope(Document document);

        //EventHandler<DocumentClosingEventArgs> DocumentClosing { get; set; }
    }
}

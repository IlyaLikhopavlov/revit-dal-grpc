using Autodesk.Revit.DB;
using Microsoft.Extensions.DependencyInjection;

namespace Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality
{
    public interface IDocumentServiceScopeFactory : IDisposable
    {
        IServiceScope CreateDocumentScope(Document document);
    }
}

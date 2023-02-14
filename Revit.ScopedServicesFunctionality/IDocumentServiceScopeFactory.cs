using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;

namespace Revit.ScopedServicesFunctionality
{
    public interface IRevitDocumentServiceScopeFactory : IServiceScopeFactory<Document>
    {
        T GetScopedService<T>(Document document) where T : class;

        object GetScopedService(Document document, Type type);
    }
}

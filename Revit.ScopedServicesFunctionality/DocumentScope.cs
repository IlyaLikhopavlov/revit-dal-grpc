using Autodesk.Revit.DB;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Revit.ScopedServicesFunctionality
{
    public class DocumentScope : Scope<Document>
    {
        public DocumentScope(IServiceScope serviceScope, Document t) : base(serviceScope, t)
        {
        }
    }
}

using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;

namespace App.ScopedServicesFunctionality
{
    internal class DocumentDescriptorScope : Scope<DocumentDescriptor>
    {
        public DocumentDescriptorScope(IServiceScope serviceScope, DocumentDescriptor t) : base(serviceScope, t)
        {
        }
    }
}

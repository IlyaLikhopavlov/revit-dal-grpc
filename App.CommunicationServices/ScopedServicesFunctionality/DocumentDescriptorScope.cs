using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;

namespace App.CommunicationServices.ScopedServicesFunctionality
{
    public class DocumentDescriptorScope : Scope<DocumentDescriptor>
    {
        public DocumentDescriptorScope(IServiceScope serviceScope, DocumentDescriptor documentDescriptor) 
            : base(serviceScope, documentDescriptor)
        {
        }
    }
}

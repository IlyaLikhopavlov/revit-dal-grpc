using Microsoft.Extensions.DependencyInjection;

namespace App.ScopedServicesFunctionality
{
    public interface IDocumentDescriptorServiceScopeFactory
    {
        IServiceScope CreateScope(DocumentDescriptor documentDescriptor);

        void OnDocumentClosing(DocumentDescriptor documentDescriptor);
    }
}

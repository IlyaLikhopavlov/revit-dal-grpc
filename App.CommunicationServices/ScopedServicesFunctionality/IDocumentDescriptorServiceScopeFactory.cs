using Microsoft.Extensions.DependencyInjection;

namespace App.CommunicationServices.ScopedServicesFunctionality
{
    public interface IDocumentDescriptorServiceScopeFactory
    {
        IServiceScope CreateScope();

        T GetScopedService<T>() where T : class;

        void RemoveScope(DocumentDescriptor documentDescriptor);
    }
}

using Microsoft.Extensions.DependencyInjection;

namespace App.CommunicationServices.ScopedServicesFunctionality
{
    public interface IDocumentDescriptorServiceScopeFactory
    {
        IServiceScope CreateScope();

        T GetScopedService<T>() where T : class;

        object GetScopedService(Type type);

        void RemoveScope(DocumentDescriptor documentDescriptor);
    }
}

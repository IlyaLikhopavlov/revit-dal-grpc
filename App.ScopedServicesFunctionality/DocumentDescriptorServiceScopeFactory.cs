using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;
using Revit.Services.Grpc.Services;

namespace App.ScopedServicesFunctionality
{
    public class DocumentDescriptorServiceScopeFactory : DocumentServiceScopeFactory<DocumentDescriptor>, IDocumentDescriptorServiceScopeFactory
    {
        public DocumentDescriptorServiceScopeFactory(IServiceScopeFactory serviceScopeFactory) : base(serviceScopeFactory)
        {
        }

        public void OnDocumentClosing(DocumentDescriptor documentDescriptor)
        {
            ScopeDictionary.TryRemove(documentDescriptor, out var scope);
        }
    }
}

using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;
using Revit.Services.Grpc.Services;

namespace App.ScopedServicesFunctionality
{
    public class DocumentDescriptorServiceScopeFactory : IDocumentDescriptorServiceScopeFactory
    {
        private readonly DocumentServiceScopeFactory<DocumentDescriptor> _documentServiceScopeFactory;

        private readonly ApplicationObject _applicationObject;

        public DocumentDescriptorServiceScopeFactory(DocumentServiceScopeFactory<DocumentDescriptor> documentServiceScopeFactory, ApplicationObject applicationObject)
        {
            _documentServiceScopeFactory = documentServiceScopeFactory;
            _applicationObject = applicationObject;
        }

        public T GetScopedService<T>()
        {

        }

        public void RemoveScope(DocumentDescriptor documentDescriptor)
        {
            _documentServiceScopeFactory.RemoveScope(documentDescriptor);
        }
    }
}

using App.CommunicationServices.Revit;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;

namespace App.CommunicationServices.ScopedServicesFunctionality
{
    public class DocumentDescriptorServiceScopeFactory : IDocumentDescriptorServiceScopeFactory, IDisposable
    {
        private readonly DocumentServiceScopeFactory<DocumentDescriptor> _documentServiceScopeFactory;

        private readonly ApplicationObject _applicationObject;

        private bool _disposed;

        public DocumentDescriptorServiceScopeFactory(
            DocumentServiceScopeFactory<DocumentDescriptor> documentServiceScopeFactory, 
            ApplicationObject applicationObject)
        {
            _documentServiceScopeFactory = documentServiceScopeFactory;
            _applicationObject = applicationObject;
        }

        public IServiceScope CreateScope()
        {
            return _documentServiceScopeFactory.CreateScope(_applicationObject.ActiveDocument);
        }

        private void AssertDocumentIsInitialized()
        {
            if (_applicationObject.ActiveDocument is null)
            {
                throw new InvalidOperationException("Scope object isn't initialized.");
            }
        }

        public T GetScopedService<T>() where T : class 
        {
            AssertDocumentIsInitialized();

            var scope = CreateScope();

            return scope.ServiceProvider.GetService<IFactory<DocumentDescriptor, T>>()?
                              .New(((Scope<DocumentDescriptor>)scope).ScopeObject) 
                          ?? throw new InvalidOperationException($"Required service {typeof(T).Name} didn't find");
        }

        public object GetScopedService(Type type)
        {
            AssertDocumentIsInitialized();

            var scope = CreateScope();

            var factoryType = FactoryUtils.ConstructGenericFactoryType(typeof(DocumentDescriptor), type);
            var factory = scope.ServiceProvider.GetService(factoryType);

            if (factory is null)
            {
                throw new InvalidOperationException($"Required service type {factoryType} didn't find.");
            }

            var scopeObject = ((Scope<DocumentDescriptor>)scope).ScopeObject;

            if (scopeObject is null)
            {
                throw new InvalidOperationException("Scope object hasn't initialized yet.");
            }

            return
                factory
                    .GetType()
                    .GetMethod("New")
                    ?.Invoke(factory, new object[] { scopeObject });
        }

        public void RemoveScope(DocumentDescriptor documentDescriptor)
        {
            _documentServiceScopeFactory.RemoveScope(documentDescriptor);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _documentServiceScopeFactory?.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

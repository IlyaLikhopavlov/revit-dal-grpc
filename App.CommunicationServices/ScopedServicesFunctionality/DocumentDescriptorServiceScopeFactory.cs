using App.CommunicationServices.Revit;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;

namespace App.CommunicationServices.ScopedServicesFunctionality
{
    public class DocumentDescriptorServiceScopeFactory : IDocumentDescriptorServiceScopeFactory, IDisposable
    {
        private readonly ServiceScopeFactory<DocumentDescriptor> _serviceScopeFactory;

        private readonly ApplicationObject _applicationObject;

        private bool _disposed;

        public DocumentDescriptorServiceScopeFactory(
            ServiceScopeFactory<DocumentDescriptor> serviceScopeFactory, 
            ApplicationObject applicationObject)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _applicationObject = applicationObject;
        }

        public IServiceScope CreateScope()
        {
            return _serviceScopeFactory.CreateScope(_applicationObject.ActiveDocument);
        }

        private void AssertDocumentIsInitialized()
        {
            if (_applicationObject.ActiveDocument is null)
            {
                throw new InvalidOperationException("Scope object isn't initialized.");
            }
        }

        private static DocumentDescriptor GetScopeObject(IServiceScope serviceScope)
        {
            var scopeObject = ((Scope<DocumentDescriptor>)serviceScope).ScopeObject;

            if (scopeObject is null)
            {
                throw new InvalidOperationException("Scope object hasn't been initialized yet.");
            }

            return scopeObject;
        }

        public T GetScopedService<T>() where T : class 
        {
            AssertDocumentIsInitialized();

            var scope = CreateScope();
            var scopeObject = GetScopeObject(scope);

            return scope.ServiceProvider.GetService<IFactory<DocumentDescriptor, T>>()?.New(scopeObject) 
                          ?? throw new InvalidOperationException($"Required service {typeof(T).Name} wasn't found");
        }

        public object GetScopedService(Type serviceType, object constructorArg = null)
        {
            AssertDocumentIsInitialized();

            var scope = CreateScope();

            var factoryType = constructorArg == null 
                ? FactoryUtils.ConstructGenericFactoryType(typeof(DocumentDescriptor), serviceType) 
                : FactoryUtils.ConstructGenericFactoryType(typeof(DocumentDescriptor), constructorArg.GetType(), serviceType);
            
            var factory = scope.ServiceProvider.GetService(factoryType);

            if (factory is null)
            {
                throw new InvalidOperationException($"Required service type {factoryType} wasn't found.");
            }

            var scopeObject = GetScopeObject(scope);

            return constructorArg == null 
                    ? factory
                        .GetType()
                        .GetMethod("New")
                        ?.Invoke(factory, new object[] { scopeObject })
                    : factory
                        .GetType()
                        .GetMethod("New")
                        ?.Invoke(factory, new [] { scopeObject, constructorArg });
        }

        public void RemoveScope(DocumentDescriptor documentDescriptor)
        {
            _serviceScopeFactory.RemoveScope(documentDescriptor);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _serviceScopeFactory?.Dispose();
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

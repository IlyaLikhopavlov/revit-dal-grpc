using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Revit.ScopedServicesFunctionality
{
    public class RevitDocumentServiceScopeFactory : IRevitDocumentServiceScopeFactory
    {
        private readonly ServiceScopeFactory<Document> _serviceScopeFactory;

        public RevitDocumentServiceScopeFactory(ServiceScopeFactory<Document> serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IServiceScope CreateScope(Document document)
        {
            if (document.IsFamilyDocument)
            {
                return null;
            }

            var result = _serviceScopeFactory.CreateScope(document);

            document.DocumentClosing += DocumentOnDocumentClosing;
            return result;

        }

        private void DocumentOnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            if (_serviceScopeFactory.RemoveScope(e.Document))
            {
                e.Document.DocumentClosing -= DocumentOnDocumentClosing;
            }
        }

        private static Document GetScopeObject(IServiceScope serviceScope)
        {
            var scopeObject = ((Scope<Document>)serviceScope).ScopeObject;

            if (scopeObject is null)
            {
                throw new InvalidOperationException("Scope object hasn't been initialized yet.");
            }

            return scopeObject;
        }

        public T GetScopedService<T>(Document document) where T : class
        {
            var scope = CreateScope(document);
            var scopeObject = GetScopeObject(scope);

            return scope.ServiceProvider.GetService<IFactory<Document, T>>()?.New(scopeObject)
                   ?? throw new InvalidOperationException($"Required service {typeof(T).Name} wasn't found");

        }

        public object GetScopedService(Document document, Type type)
        {
            var scope = CreateScope(document);

            var factoryType = FactoryUtils.ConstructGenericFactoryType(typeof(Document), type);
            var factory = scope.ServiceProvider.GetService(factoryType);

            if (factory is null)
            {
                throw new InvalidOperationException($"Required service type {factoryType} wasn't found.");
            }

            var scopeObject = GetScopeObject(scope);

            return
                factory
                    .GetType()
                    .GetMethod("New")
                    ?.Invoke(factory, new object[] { scopeObject });
        }

        private bool _disposed;

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

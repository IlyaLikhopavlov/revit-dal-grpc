using System.Collections.Concurrent;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Microsoft.Extensions.DependencyInjection;

namespace Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality
{
    public class DocumentServiceScopeFactory : IDocumentServiceScopeFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ConcurrentDictionary<Document, DocumentScope> _scopeDictionary = new();

        private bool _disposed;

        public DocumentServiceScopeFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IServiceScope CreateDocumentScope(Document document)
        {
            if (document.IsFamilyDocument)
            {
                return null;
            }

            if (_scopeDictionary.TryGetValue(document, out var scope))
            {
                return scope;
            }

            var newScope = new DocumentScope(_serviceScopeFactory.CreateScope(), document);
            if (!_scopeDictionary.TryAdd(document, newScope))
            {
                return null;
            }

            document.DocumentClosing += DocumentOnDocumentClosing;
            return newScope;

        }

        private void DocumentOnDocumentClosing(object sender, DocumentClosingEventArgs e)
        {
            if (_scopeDictionary.TryRemove(e.Document, out var scope))
            {
                scope.Document.DocumentClosing -= DocumentOnDocumentClosing;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                
                foreach (var scope in _scopeDictionary.Values)
                {
                    scope.Dispose();
                }

                _scopeDictionary.Clear();
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

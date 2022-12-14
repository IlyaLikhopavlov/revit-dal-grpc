using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Revit.Services.Grpc.Services;

namespace App.Grpc.Bundle.ScopedServicesFunctionality
{
    public class DocumentServiceScopeFactory : IDocumentServiceScopeFactory
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly ConcurrentDictionary<DocumentDescriptor, DocumentDescriptorScope> _scopeDictionary = new();

        private bool _disposed;

        public DocumentServiceScopeFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public IServiceScope CreateScope(DocumentDescriptor document)
        {
            if (_scopeDictionary.TryGetValue(document, out var scope))
            {
                return scope;
            }

            var newScope = new DocumentDescriptorScope(_serviceScopeFactory.CreateScope(), document);
            return _scopeDictionary.TryAdd(document, newScope) ? newScope : null;
        }

        public void OnDocumentClosing(DocumentDescriptor documentDescriptor)
        {
            _scopeDictionary.TryRemove(documentDescriptor, out var scope);
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

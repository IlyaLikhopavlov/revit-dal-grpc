using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base
{
    public class DocumentServiceScopeFactory<T> : IDocumentServiceScopeFactory<T>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected readonly ConcurrentDictionary<T, IServiceScope> ScopeDictionary = new();

        private bool _disposed;

        public DocumentServiceScopeFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public virtual IServiceScope CreateScope(T t)
        {
            if (ScopeDictionary.TryGetValue(t, out var scope))
            {
                return scope;
            }

            var newScope = Scope<T>.CreateScope(_serviceScopeFactory.CreateScope(), t);
            return ScopeDictionary.TryAdd(t, newScope) ? newScope : null;
        }

        public void RemoveScope(T t)
        {
            ScopeDictionary.TryRemove(t, out var scope);
            scope.Dispose();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {

                foreach (var scope in ScopeDictionary.Values)
                {
                    scope.Dispose();
                }

                ScopeDictionary.Clear();
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

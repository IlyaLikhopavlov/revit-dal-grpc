using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

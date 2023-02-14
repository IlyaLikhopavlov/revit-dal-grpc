using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base
{
    public class ServiceScopeFactory<T> : IServiceScopeFactory<T>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        protected readonly ConcurrentDictionary<T, IServiceScope> ScopeDictionary;

        private bool _disposed;

        public ServiceScopeFactory(IServiceScopeFactory serviceScopeFactory, IEqualityComparer<T> equalityComparer)
        {
            _serviceScopeFactory = serviceScopeFactory;
            ScopeDictionary = new ConcurrentDictionary<T, IServiceScope>(equalityComparer);
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

        public bool RemoveScope(T t)
        {
            if (!ScopeDictionary.TryRemove(t, out var scope))
            {
                return false;
            }

            scope.Dispose();
            _disposed = true;
            return true;
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

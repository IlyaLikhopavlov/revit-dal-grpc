using Microsoft.Extensions.DependencyInjection;

namespace Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base
{
    public class Scope<T> : IServiceScope
    {
        public IServiceProvider ServiceProvider => _serviceScope.ServiceProvider;

        private readonly IServiceScope _serviceScope;

        public Scope(IServiceScope serviceScope, T t)
        {
            _serviceScope = serviceScope;
            ScopeObject = t;
        }

        public static IServiceScope CreateScope(IServiceScope serviceScope, T t)
        {
            return new Scope<T>(serviceScope, t);
        }

        public T ScopeObject { get; }

        public void Dispose()
        {
            _serviceScope?.Dispose();
        }
    }
}

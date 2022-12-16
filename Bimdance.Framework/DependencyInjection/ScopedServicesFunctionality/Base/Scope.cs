using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

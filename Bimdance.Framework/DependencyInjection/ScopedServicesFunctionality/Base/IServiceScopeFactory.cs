using Microsoft.Extensions.DependencyInjection;

namespace Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base
{
    public interface IServiceScopeFactory<in T> : IDisposable
    {
        IServiceScope CreateScope(T t);
    }
}

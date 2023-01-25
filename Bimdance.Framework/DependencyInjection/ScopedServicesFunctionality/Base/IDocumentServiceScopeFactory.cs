using Microsoft.Extensions.DependencyInjection;

namespace Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base
{
    public interface IDocumentServiceScopeFactory<in T> : IDisposable
    {
        IServiceScope CreateScope(T t);
    }
}

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bimdance.Framework.DependencyInjection.ScopedServicesFunctionality.Base
{
    public interface IDocumentServiceScopeFactory<in T> : IDisposable
    {
        IServiceScope CreateScope(T t);
    }
}

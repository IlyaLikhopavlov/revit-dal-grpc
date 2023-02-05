using System;
using System.Collections.Generic;
using System.Text;

namespace Bimdance.Framework.DependencyInjection.FactoryFunctionality
{
    internal class Factory<T1, T2, TService> : FactoryBase<TService>, IFactory<T1, T2, TService>
    {
        public Factory(IServiceProvider serviceProvider, ServiceMap serviceMap)
            : base(serviceProvider, serviceMap, new[] { typeof(T1), typeof(T2) })
        {
        }

        public TService New(T1 arg1, T2 arg2) => New(new object[] { arg1, arg2 });
    }
}

namespace Bimdance.Framework.DependencyInjection.FactoryFunctionality
{
    internal class Factory<T, TService> : FactoryBase<TService>, IFactory<T, TService>
    {
        public Factory(IServiceProvider serviceProvider, ServiceMap serviceMap)
            : base(serviceProvider, serviceMap, new[] { typeof(T) })
        {
        }

        public TService New(T arg) => New(new object[] { arg });
    }
}
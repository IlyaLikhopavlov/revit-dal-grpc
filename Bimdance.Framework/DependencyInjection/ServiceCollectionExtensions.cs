using Bimdance.Framework.DependencyInjection.FactoryFunctionality;
using Microsoft.Extensions.DependencyInjection;

namespace Bimdance.Framework.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLazyResolution(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddTransient(
                typeof(Lazy<>),
                typeof(LazilyResolved<>));
        }

        private class LazilyResolved<T> : Lazy<T>
        {
            public LazilyResolved(IServiceProvider serviceProvider)
                : base(serviceProvider.GetRequiredService<T>)
            {
            }
        }

        /// <summary>
        /// Add a generic factory facility for all <see cref="IFactory{TService}"/>, <see cref="IFactory{T, TService}"/>, <see cref="IFactory{T1, T2, TService}"/> etc.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" /> to add the factory service to.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IServiceCollection AddFactoryFacility(this IServiceCollection services) =>
            services.AddSingleton(new ServiceMap(services))
                .AddSingleton(typeof(IFactory<>), typeof(Factory<>))
                .AddSingleton(typeof(IFactory<,>), typeof(Factory<,>))
                .AddSingleton(typeof(IFactory<,,,,>), typeof(Factory<,,,,>));
    }
}

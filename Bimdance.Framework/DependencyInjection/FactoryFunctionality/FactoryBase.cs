using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Bimdance.Framework.DependencyInjection.FactoryFunctionality
{
    internal abstract class FactoryBase<TService>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly Type[] _argTypes;
        private readonly Type _implementationType;
        private readonly ServiceLifetime _lifetime;
        private readonly ConstructorInfo _constructor;

        protected FactoryBase(IServiceProvider serviceProvider, ServiceMap serviceMap, Type[] argTypes)
        {
            _serviceProvider = serviceProvider;
            _argTypes = argTypes;
            var serviceDescriptor = serviceMap.TryGetValue(typeof(TService), out var value)
                ? value.Single()
                : throw new InvalidOperationException($"Unable to resolve service for type '{typeof(TService)}' while attempting to activate '{GetType().GetInterfaces().Single()}'.");

            _implementationType = serviceDescriptor.ImplementationType;
            _lifetime = serviceDescriptor.Lifetime;
            if (serviceDescriptor.Lifetime == ServiceLifetime.Singleton)
            {
                throw new InvalidOperationException(
                    $"In order to resolve a parameterised factory for service type '{typeof(TService)}', the implementation type '{_implementationType}' must not be registered with Singleton lifestyle.");
            }

            var matchingConstructors = _implementationType.GetConstructors().Where(constructorInfo => IsMatch(constructorInfo, argTypes)).ToList();
            if (matchingConstructors.Count != 1)
            {
                var argTypesText = string.Join(", ", argTypes.Select(argType => $"'{argType}'"));
                throw new InvalidOperationException(
                    $"In order to resolve a parameterised factory for service type '{typeof(TService)}', the implementation type '{_implementationType}' must contain {(matchingConstructors.Count == 0 ? "a" : "just one")} constructor whose last {(argTypes.Length > 1 ? argTypes.Length + " parameters are assignable from the factory argument types" : "parameter is assignable from the factory argument type")} {argTypesText}.");
            }

            _constructor = matchingConstructors[0];
        }

        private readonly IDictionary<object, TService> _constructorCache = new ConcurrentDictionary<object, TService>();

        protected TService New(object[] argValues)
        {
            if (argValues.Length != _argTypes.Length)
            {
                // This shouldn't be possible
                throw new NotSupportedException();
            }

            var parameters = _constructor.GetParameters();
            var constructorArgs = new object[parameters.Length];
            for (var i = 0; i < parameters.Length - _argTypes.Length; i++)
            {
                // n.b. Resolve services late, in order to respect lifestyles of resolved services
                constructorArgs[i] = _serviceProvider.GetService(parameters[i].ParameterType) ??
                                     throw new InvalidOperationException($"Unable to resolve service for type '{parameters[i].ParameterType}' while attempting to activate '{_implementationType}'.");
            }

            for (var i = parameters.Length - _argTypes.Length; i < parameters.Length; i++)
            {
                constructorArgs[i] = argValues[i - parameters.Length + _argTypes.Length];
            }

            TService service;

            if (_lifetime == ServiceLifetime.Scoped)
            {
                if (_constructorCache.ContainsKey(argValues[0]))
                {
                    service = _constructorCache[argValues[0]];
                }
                else
                {
                    service = (TService)_constructor.Invoke(constructorArgs);
                    _constructorCache.Add(argValues[0], service);
                }
            }
            else
            {
                service = (TService)_constructor.Invoke(constructorArgs);
            }
            return service;
        }

        private static bool IsMatch(ConstructorInfo constructor, IReadOnlyList<Type> argTypes)
        {
            var parameters = constructor.GetParameters();
            if (parameters.Length < argTypes.Count)
            {
                return false;
            }

            for (var i = 0; i < argTypes.Count; i++)
            {
                var argType = argTypes[i];
                var parameterType = parameters[i + parameters.Length - argTypes.Count].ParameterType;
                if (!parameterType.IsAssignableFrom(argType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
using System.Collections;
using Microsoft.Extensions.DependencyInjection;

namespace Bimdance.Framework.DependencyInjection.FactoryFunctionality
{
    internal class ServiceMap : IReadOnlyDictionary<Type, IReadOnlyList<ServiceDescriptor>>
    {
        private readonly Dictionary<Type, IReadOnlyList<ServiceDescriptor>> _serviceMap;

        public ServiceMap(IServiceCollection services) =>
            _serviceMap = services.GroupBy(service => service.ServiceType)
                .ToDictionary(
                    group => group.Key,
                    group => (IReadOnlyList<ServiceDescriptor>)group.ToList());

        public int Count => _serviceMap.Count;

        public IEnumerable<Type> Keys => _serviceMap.Keys;

        public IEnumerable<IReadOnlyList<ServiceDescriptor>> Values => _serviceMap.Values;

        public IReadOnlyList<ServiceDescriptor> this[Type index] => _serviceMap[index];

        public bool ContainsKey(Type key) => _serviceMap.ContainsKey(key);

        public bool TryGetValue(Type key, out IReadOnlyList<ServiceDescriptor> value) => _serviceMap.TryGetValue(key, out value);

        public IEnumerator<KeyValuePair<Type, IReadOnlyList<ServiceDescriptor>>> GetEnumerator() => _serviceMap.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
namespace Bimdance.Framework.DependencyInjection.FactoryFunctionality
{
    public static class FactoryUtils
    {
        public static Type ConstructGenericFactoryType(params Type[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Length < 1)
            {
                throw new ArgumentException(nameof(args));
            }

            var factoryType = args.Length switch
            {
                1 => typeof(IFactory<>),
                2 => typeof(IFactory<,>),
                3 => typeof(IFactory<,,>),
                4 => typeof(IFactory<,,,>),
                5 => typeof(IFactory<,,,,>),
                _ => throw new ArgumentOutOfRangeException(nameof(args))
            };

            return factoryType.MakeGenericType(args);
        }
    }
}

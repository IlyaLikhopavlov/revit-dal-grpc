using System;
using System.Collections.Generic;
using System.Text;

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
                1 => typeof(Factory<>),
                2 => typeof(Factory<,>),
                3 => typeof(Factory<,,>),
                4 => typeof(Factory<,,,>),
                5 => typeof(Factory<,,,,>),
                _ => throw new ArgumentOutOfRangeException(nameof(args))
            };

            return factoryType.MakeGenericType(args);
        }
    }
}

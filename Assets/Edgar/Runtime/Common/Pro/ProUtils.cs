using System;
using System.Collections.Generic;
using System.Linq;

namespace Edgar.Unity
{
    public static class ProUtils
    {
        public static List<Type> FindDerivedTypes(Type baseType)
        {
            return AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => baseType.IsAssignableFrom(x) && !x.IsAbstract)
                .ToList();
        }
    }
}
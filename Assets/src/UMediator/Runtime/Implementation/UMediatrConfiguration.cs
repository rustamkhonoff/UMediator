using System;
using System.Collections.Generic;
using System.Reflection;

namespace UMediator
{
    public class UMediatrConfiguration
    {
        public List<Assembly> TargetAssemblies { get; } = new();

        /// <summary>
        /// Adds assembly which contains given type
        /// </summary>
        public UMediatrConfiguration AddAssemblyWith<T>()
        {
            return AddAssemblyWithType(typeof(T));
        }

        /// <summary>
        /// Adds assembly which contains given type
        /// </summary>
        public UMediatrConfiguration AddAssemblyWithType(Type type)
        {
            return AddAssembly(type.Assembly);
        }

        /// <summary>
        /// Adds assembly to target assemblies list
        /// </summary>
        public UMediatrConfiguration AddAssembly(Assembly assembly)
        {
            TargetAssemblies.Add(assembly);

            return this;
        }

        /// <summary>
        /// Adds assembly to target assemblies list
        /// </summary>
        public UMediatrConfiguration AddAssemblies(params Assembly[] assemblies)
        {
            TargetAssemblies.AddRange(assemblies);

            return this;
        }
    }
}
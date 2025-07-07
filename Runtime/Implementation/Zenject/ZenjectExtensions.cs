#if ZENJECT
using System;
using System.Linq;
using System.Reflection;
using UMediator.Interfaces;
using UMediator.Internal;
using Zenject;

namespace UMediator.Implementation.Zenject
{
    public static class ZenjectExtensions
    {
        public static void AddUMediator(this DiContainer diContainer, params Type[] types)
        {
            UMediatrConfiguration configuration = new();
            foreach (Type type in types)
                configuration.AddAssemblyWithType(type);
            AddUMediator(diContainer, configuration);
        }

        public static void AddUMediator(this DiContainer diContainer, params Assembly[] assembly)
        {
            UMediatrConfiguration configuration = new();
            configuration.AddAssemblies(assembly);
            AddUMediator(diContainer, configuration);
        }

        public static void AddUMediator(this DiContainer diContainer, Action<UMediatrConfiguration> configuration)
        {
            UMediatrConfiguration config = new();
            configuration.Invoke(config);
            AddUMediator(diContainer, config);
        }

        private static void AddUMediator(this DiContainer diContainer, UMediatrConfiguration configuration)
        {
            if (!configuration.TargetAssemblies.Any())
                throw new ArgumentException("There is no assemblies to scan!");

            IUMediatrHandlersCollection collection =
                HandlersScanner.ScanHandlers(configuration.TargetAssemblies.ToArray());
            diContainer
                .Bind<IUMediatrHandlersCollection>()
                .FromInstance(collection)
                .AsSingle();


            if (configuration.CustomMediatorTypeFactory == null)
            {
                diContainer
                    .BindInterfacesAndSelfTo<ZenjectMediatorTypeFactory>()
                    .AsSingle();
            }
            else
            {
                diContainer
                    .Bind<IMediatorTypeFactory>()
                    .FromInstance(configuration.CustomMediatorTypeFactory)
                    .AsSingle();
            }

            diContainer
                .Bind<IMediator>()
                .To<Mediator>()
                .AsSingle()
                .NonLazy();
        }
    }
}
#endif
#if ZENJECT
using System;
using System.Linq;
using System.Reflection;
using Zenject;

namespace UMediator.Zenject
{
    public static class Extensions
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

            foreach (Type type in collection.NotificationHandlerTypes.Values.SelectMany(value => value))
                diContainer.BindInterfacesTo(type).AsCached();

            foreach (Type type in collection.RequestHandlerTypes.Values)
                diContainer.BindInterfacesTo(type).AsCached();

            diContainer
                .BindInterfacesTo<ServiceProvider>()
                .AsSingle();

            diContainer
                .Bind<IMediator>()
                .To<Mediator>()
                .AsSingle()
                .NonLazy();
        }
    }
}
#endif
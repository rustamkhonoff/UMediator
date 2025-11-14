#if REFLEX
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Reflex.Core;

namespace UMediator.Reflex
{
    public static class Extensions
    {
        public static void AddUMediator(this ContainerBuilder container, params Type[] types)
        {
            UMediatrConfiguration configuration = new();
            foreach (Type type in types)
                configuration.AddAssemblyWithType(type);
            AddUMediator(container, configuration);
        }

        public static void AddUMediator(this ContainerBuilder container, params Assembly[] assembly)
        {
            UMediatrConfiguration configuration = new();
            configuration.AddAssemblies(assembly);
            AddUMediator(container, configuration);
        }

        public static void AddUMediator(this ContainerBuilder container, Action<UMediatrConfiguration> configuration)
        {
            UMediatrConfiguration config = new();
            configuration.Invoke(config);
            AddUMediator(container, config);
        }

        public static void AddUMediator(this ContainerBuilder builder, UMediatrConfiguration configuration)
        {
            if (!configuration.TargetAssemblies.Any())
                throw new ArgumentException("There is no assemblies to scan!");

            IUMediatrHandlersCollection collection =
                HandlersScanner.ScanHandlers(configuration.TargetAssemblies.ToArray());

            foreach ((Type notificationType, HashSet<Type> value) in collection.NotificationHandlerTypes)
            {
                foreach (Type handlerType in value)
                {
                    Type handlerInterface = typeof(INotificationHandler<>).MakeGenericType(notificationType);
                    builder.AddTransient(handlerType, handlerInterface);
                }
            }

            foreach ((Type requestType, Type handlerType) in collection.RequestHandlerTypes)
            {
                Type handlerInterface = handlerType
                    .GetInterfaces()
                    .First(i => i.IsGenericType &&
                                (i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                                 i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)));

                builder.AddTransient(handlerType, handlerInterface);
            }

            builder.AddSingleton(typeof(Mediator), typeof(IMediator), typeof(ISender), typeof(IPublisher));
            builder.AddSingleton(typeof(ServiceProvider), typeof(IMediatorServiceProvider));
        }
    }
}
#endif
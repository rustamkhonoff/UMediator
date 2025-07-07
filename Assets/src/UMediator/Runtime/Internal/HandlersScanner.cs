using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace UMediator
{
    public static class HandlersScanner
    {
        public static IUMediatrHandlersCollection ScanHandlers(params Assembly[] assemblies)
        {
            Dictionary<Type, HashSet<Type>> notificationHandlerTypes = new();
            Dictionary<Type, Type> requestHandlerTypes = new();

            foreach (Assembly assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => t.GetInterfaces().Any(i => i.IsGenericType &&
                    (i.GetGenericTypeDefinition() ==
                     typeof(INotificationHandler<>) ||
                     i.GetGenericTypeDefinition() == typeof(IRequestHandler<>) ||
                     i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>))));

                foreach (Type type in types)
                {
                    var handlerInterfaces = type.GetInterfaces().Where(i => i.IsGenericType);
                    foreach (Type handlerInterface in handlerInterfaces)
                    {
                        Type genericType = handlerInterface.GetGenericTypeDefinition();
                        var genericArguments = handlerInterface.GetGenericArguments();

                        if (genericType == typeof(INotificationHandler<>))
                        {
                            Type notificationType = genericArguments[0];
                            if (!notificationHandlerTypes.ContainsKey(notificationType))
                            {
                                notificationHandlerTypes[notificationType] = new HashSet<Type>();
                            }

                            notificationHandlerTypes[notificationType].Add(type);
                        }
                        else if (genericType == typeof(IRequestHandler<,>) || genericType == typeof(IRequestHandler<>))
                        {
                            Type requestType = genericArguments[0];
                            if (requestHandlerTypes.ContainsKey(requestType))
                            {
                                Debug.LogError(
                                    $"Request handler {requestHandlerTypes[requestType]} for type {requestType} already defined, ignoring {type}");
                                continue;
                            }

                            requestHandlerTypes[requestType] = type;
                        }
                    }
                }
            }

            return new UMediatrHandlersCollection(notificationHandlerTypes, requestHandlerTypes);
        }
    }
}
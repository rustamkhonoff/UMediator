using System;
using System.Collections.Generic;

namespace UMediator.Internal
{
    public class UMediatrHandlersCollection : IUMediatrHandlersCollection
    {
        public UMediatrHandlersCollection(Dictionary<Type, List<Type>> notificationHandlerTypes,
            Dictionary<Type, Type> requestHandlerTypes)
        {
            NotificationHandlerTypes = notificationHandlerTypes;
            RequestHandlerTypes = requestHandlerTypes;
        }

        public Dictionary<Type, List<Type>> NotificationHandlerTypes { get; }

        public Dictionary<Type, Type> RequestHandlerTypes { get; }
    }
}
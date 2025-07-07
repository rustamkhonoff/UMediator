using System;
using System.Collections.Generic;

namespace UMediator.Internal
{
    public class UMediatrHandlersCollection : IUMediatrHandlersCollection
    {
        public UMediatrHandlersCollection(Dictionary<Type, HashSet<Type>> notificationHandlerTypes,
            Dictionary<Type, Type> requestHandlerTypes)
        {
            NotificationHandlerTypes = notificationHandlerTypes;
            RequestHandlerTypes = requestHandlerTypes;
        }

        public Dictionary<Type, HashSet<Type>> NotificationHandlerTypes { get; }

        public Dictionary<Type, Type> RequestHandlerTypes { get; }
    }
}
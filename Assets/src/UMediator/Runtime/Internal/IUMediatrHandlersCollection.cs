using System;
using System.Collections.Generic;

namespace UMediator.Internal
{
    public interface IUMediatrHandlersCollection
    {
        Dictionary<Type, HashSet<Type>> NotificationHandlerTypes { get; }
        Dictionary<Type, Type> RequestHandlerTypes { get; }
    }
}
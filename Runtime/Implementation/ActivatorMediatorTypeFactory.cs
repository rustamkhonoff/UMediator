using System;
using UMediator.Interfaces;

namespace UMediator.Implementation
{
    public sealed class ActivatorMediatorTypeFactory : IMediatorTypeFactory
    {
        public object CreateInstanceFor(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
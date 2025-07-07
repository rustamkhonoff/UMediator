using System;

namespace UMediator
{
    public sealed class ActivatorMediatorTypeFactory : IMediatorTypeFactory
    {
        public object CreateInstanceFor(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}
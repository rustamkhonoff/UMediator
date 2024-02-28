using System;

namespace UMediator.Interfaces
{
    public interface IMediatorTypeFactory
    {
        object CreateInstanceFor(Type type);
    }
}
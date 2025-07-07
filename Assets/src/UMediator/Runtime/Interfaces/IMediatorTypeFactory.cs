using System;

namespace UMediator
{
    public interface IMediatorTypeFactory
    {
        object CreateInstanceFor(Type type);
    }
}
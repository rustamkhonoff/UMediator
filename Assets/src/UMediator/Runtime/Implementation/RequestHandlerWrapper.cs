using UMediator.Sender;

namespace UMediator.Implementation
{
    internal abstract class RequestHandlerWrapper<TResponse> : RequestHandlerBase
    {
        public abstract TResponse Handle(IRequest<TResponse> request, object handler);
    }

    internal abstract class RequestHandlerWrapper : RequestHandlerBase
    {
        public abstract Unit Handle(IRequest request, object handler);
    }
}
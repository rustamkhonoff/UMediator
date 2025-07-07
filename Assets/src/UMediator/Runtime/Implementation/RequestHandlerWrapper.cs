using Cysharp.Threading.Tasks;

namespace UMediator
{
    internal abstract class RequestHandlerWrapper<TResponse> : RequestHandlerBase
    {
        public abstract UniTask<TResponse> Handle(IRequest<TResponse> request, object handler);
    }

    internal abstract class RequestHandlerWrapper : RequestHandlerBase
    {
        public abstract UniTask Handle(IRequest request, object handler);
    }
}
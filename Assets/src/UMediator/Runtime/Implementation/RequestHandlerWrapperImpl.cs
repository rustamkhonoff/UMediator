using Cysharp.Threading.Tasks;

namespace UMediator
{
    internal class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override UniTask<TResponse> Handle(IRequest<TResponse> request, object handler)
        {
            return ((IRequestHandler<TRequest, TResponse>)handler).Handle((TRequest)request);
        }

        public override async UniTask<object> Handle(object request, object handler)
        {
            TResponse result = await Handle((IRequest<TResponse>)request, handler);
            return result;
        }
    }

    internal class RequestHandlerWrapperImpl<TRequest> : RequestHandlerWrapper where TRequest : IRequest
    {
        public override async UniTask<object> Handle(object request, object handler)
        {
            await Handle((IRequest)request, handler);
            return Unit.Empty;
        }

        public override UniTask Handle(IRequest request, object handler)
        {
            return ((IRequestHandler<TRequest>)handler).Handle((TRequest)request);
        }
    }
}
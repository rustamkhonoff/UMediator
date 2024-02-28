using UMediator.Sender;

namespace UMediator.Implementation
{
    internal class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override TResponse Handle(IRequest<TResponse> request, object handler)
        {
            return ((IRequestHandler<TRequest, TResponse>)handler).Handle((TRequest)request);
        }

        public override object Handle(object request, object handler)
        {
            return Handle((IRequest<TResponse>)request, handler);
        }
    }

    internal class RequestHandlerWrapperImpl<TRequest> : RequestHandlerWrapper where TRequest : IRequest
    {
        public override object Handle(object request, object handler)
        {
            return Handle((TRequest)request, handler);
        }

        public override Unit Handle(IRequest request, object handler)
        {
            ((IRequestHandler<TRequest>)handler).Handle((TRequest)request);
            return Unit.Empty;
        }
    }
}
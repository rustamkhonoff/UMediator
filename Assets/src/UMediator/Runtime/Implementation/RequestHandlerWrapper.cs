using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UMediator.Pipeline;

namespace UMediator
{
    public abstract class RequestHandlerWrapperBase
    {
    }

    public abstract class RequestHandlerWrapper<TResponse> : RequestHandlerWrapperBase
    {
        public abstract UniTask<TResponse> Handle(
            IRequest<TResponse> request,
            IMediatorServiceProvider provider,
            CancellationToken ct);
    }

    public abstract class RequestHandlerWrapper : RequestHandlerWrapperBase
    {
        public abstract UniTask Handle(
            IRequest request,
            IMediatorServiceProvider provider,
            CancellationToken ct);
    }

    internal class RequestHandlerWrapperImpl<TRequest, TResponse> : RequestHandlerWrapper<TResponse>
        where TRequest : IRequest<TResponse>
    {
        public override async UniTask<TResponse> Handle(IRequest<TResponse> request, IMediatorServiceProvider provider,
            CancellationToken ct)
        {
            var handler = provider.GetService<IRequestHandler<TRequest, TResponse>>();
            if (handler == null)
                throw new InvalidOperationException($"Handler not found for {typeof(TRequest).Name}");

            var behaviors = provider.GetService<IEnumerable<IPipelineBehavior<TRequest, TResponse>>>()
                            ?? Enumerable.Empty<IPipelineBehavior<TRequest, TResponse>>();

            var pipeline = behaviors.Reverse()
                .Aggregate((RequestHandlerDelegate<TResponse>)HandlerDelegate, (next, behavior) =>
                    token => behavior.Handle((TRequest)request, next, token));

            return await pipeline(ct);

            UniTask<TResponse> HandlerDelegate(CancellationToken token) => handler.Handle((TRequest)request, token);
        }
    }

    internal class RequestHandlerWrapperImpl<TRequest> : RequestHandlerWrapper
        where TRequest : IRequest
    {
        public override async UniTask Handle(
            IRequest request,
            IMediatorServiceProvider provider,
            CancellationToken ct)
        {
            var handler = provider.GetService<IRequestHandler<TRequest>>();
            if (handler == null)
                throw new InvalidOperationException($"Handler not found for {typeof(TRequest).Name}");

            var behaviors = provider.GetService<IEnumerable<IPipelineBehavior<TRequest>>>()
                            ?? Enumerable.Empty<IPipelineBehavior<TRequest>>();

            RequestHandlerDelegate pipeline = behaviors.Reverse()
                .Aggregate((RequestHandlerDelegate)HandlerDelegate, (next, behavior) =>
                    token => behavior.Handle((TRequest)request, next, token));

            await pipeline(ct);
            return;

            UniTask HandlerDelegate(CancellationToken token) => handler.Handle((TRequest)request, token);
        }
    }
}
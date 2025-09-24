using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UMediator.Pipeline;

namespace UMediator
{
    public interface IMediatorServiceProvider
    {
        T GetService<T>();
    }

    public class Mediator : IMediator
    {
        private readonly IMediatorServiceProvider m_serviceProvider;
        private readonly Dictionary<Type, RequestHandlerWrapperBase> m_wrappers = new();

        public Mediator(IMediatorServiceProvider serviceProvider)
        {
            m_serviceProvider = serviceProvider;
        }

        public async UniTask Publish<T>(T notification, CancellationToken ct = default) where T : INotification
        {
            if (notification == null) throw new ArgumentNullException(nameof(notification));

            var behaviors = m_serviceProvider.GetService<IEnumerable<INotificationBehavior<T>>>().ToArray();

            var handlers = m_serviceProvider.GetService<IEnumerable<INotificationHandler<T>>>();

            foreach (var handler in handlers)
            {
                NotificationHandlerDelegate pipeline = PipelineBuilder(() => handler.Handle(notification, ct), ct);
                await pipeline();
            }

            return;

            NotificationHandlerDelegate PipelineBuilder(NotificationHandlerDelegate last, CancellationToken token) =>
                behaviors
                    .Reverse()
                    .Aggregate(last, (next, behavior) => () => behavior.Handle(notification, next, token));
        }

        public async UniTask<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken ct = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            Type requestType = request.GetType();

            if (!m_wrappers.TryGetValue(requestType, out RequestHandlerWrapperBase wrapper))
            {
                Type wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(requestType, typeof(TResponse));

                wrapper = (RequestHandlerWrapperBase)Activator.CreateInstance(wrapperType)!;
                m_wrappers[requestType] = wrapper;
            }

            return await ((RequestHandlerWrapper<TResponse>)wrapper).Handle(request, m_serviceProvider, ct);
        }

        public async UniTask Send<T>(T request, CancellationToken ct = default) where T : IRequest
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            Type requestType = request.GetType();

            if (!m_wrappers.TryGetValue(requestType, out RequestHandlerWrapperBase wrapper))
            {
                Type wrapperType = typeof(RequestHandlerWrapperImpl<>).MakeGenericType(requestType);

                wrapper = (RequestHandlerWrapperBase)Activator.CreateInstance(wrapperType)!;
                m_wrappers[requestType] = wrapper;
            }

            await ((RequestHandlerWrapper)wrapper).Handle(request, m_serviceProvider, ct);
        }
    }
}
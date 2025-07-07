using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace UMediator
{
    public class Mediator : IMediator
    {
        private readonly IMediatorTypeFactory m_typeFactory;
        private readonly Dictionary<Type, HashSet<Type>> m_notificationHandlerTypes;
        private readonly Dictionary<Type, Type> m_requestHandlerTypes;
        private readonly Dictionary<Type, object> m_handlerInstances;
        private readonly Dictionary<Type, RequestHandlerBase> m_requestHandlers;

        public Mediator(IUMediatrHandlersCollection handlersCollection, IMediatorTypeFactory mediatorTypeFactory)
        {
            m_handlerInstances = new Dictionary<Type, object>();
            m_requestHandlers = new Dictionary<Type, RequestHandlerBase>();

            m_notificationHandlerTypes = handlersCollection.NotificationHandlerTypes;
            m_requestHandlerTypes = handlersCollection.RequestHandlerTypes;
            m_typeFactory = mediatorTypeFactory;
        }

        public async UniTask Publish<T>(T notification) where T : INotification
        {
            Type notificationType = typeof(T);
            if (!m_notificationHandlerTypes.TryGetValue(notificationType, out HashSet<Type> handlerTypes))
            {
                Debug.LogWarning($"There is not Handlers for Notification {notificationType.Name}");
                return;
            }

            foreach (object handler in handlerTypes.Select(GetOrCreateHandlerInstance))
                await ((INotificationHandler<T>)handler).Handle(notification);
        }

        public async UniTask<TResponse> Send<TResponse>(IRequest<TResponse> request)
        {
            Type requestType = request.GetType();

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!m_requestHandlerTypes.TryGetValue(requestType, out Type handlerType))
                throw new InvalidOperationException($"Handler not found for request type {requestType.Name}");

            if (!m_requestHandlers.TryGetValue(requestType, out RequestHandlerBase wrapperInstance))
            {
                Type wrapperType = typeof(RequestHandlerWrapperImpl<,>).MakeGenericType(requestType, typeof(TResponse));
                RequestHandlerBase wrapper = (RequestHandlerBase)Activator.CreateInstance(wrapperType);
                wrapperInstance = wrapper;
                m_requestHandlers.Add(requestType, wrapper);
            }

            object handlerInstance = GetOrCreateHandlerInstance(handlerType);

            object result = await wrapperInstance.Handle(request, handlerInstance);
            return (TResponse)result;
        }

        public async UniTask Send<T>(T request) where T : IRequest
        {
            Type requestType = typeof(T);

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!m_requestHandlerTypes.TryGetValue(requestType, out Type handlerType))
                throw new InvalidOperationException($"Handler not found for request type {requestType.Name}");

            if (!m_requestHandlers.TryGetValue(requestType, out RequestHandlerBase wrapperInstance))
            {
                Type wrapperType = typeof(RequestHandlerWrapperImpl<>).MakeGenericType(requestType);
                RequestHandlerBase wrapper = (RequestHandlerBase)Activator.CreateInstance(wrapperType);
                wrapperInstance = wrapper;
                m_requestHandlers.Add(requestType, wrapper);
            }

            object handlerInstance = GetOrCreateHandlerInstance(handlerType);

            await wrapperInstance.Handle(request, handlerInstance);
        }

        private object GetOrCreateHandlerInstance(Type handlerType)
        {
            if (m_handlerInstances.TryGetValue(handlerType, out object handler))
                return handler;

            handler = m_typeFactory.CreateInstanceFor(handlerType);
            m_handlerInstances[handlerType] = handler;

            return handler;
        }
    }
}
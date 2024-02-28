using System;
using System.Collections.Generic;
using System.Linq;
using UMediator.Interfaces;
using UMediator.Internal;
using UMediator.Publisher;
using UMediator.Sender;

namespace UMediator.Implementation
{
    public class Mediator : IMediator
    {
        private readonly IMediatorTypeFactory m_typeFactory;
        private readonly Dictionary<Type, List<Type>> m_notificationHandlerTypes;
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

        public void Publish<T>(T notification) where T : INotification
        {
            Type notificationType = typeof(T);
            if (!m_notificationHandlerTypes.TryGetValue(notificationType, out var handlerTypes))
                return;

            foreach (object handler in handlerTypes.Select(GetOrCreateHandlerInstance))
                ((INotificationHandler<T>)handler).Handle(notification);
        }

        public TResponse Send<TResponse>(IRequest<TResponse> request)
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

            return (TResponse)wrapperInstance.Handle(request, handlerInstance);
        }

        public void Send<T>(T request) where T : IRequest
        {
            Type requestType = request.GetType();

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
            wrapperInstance.Handle(request, handlerInstance);
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
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UMediator;
using UMediator.Pipeline;
using UnityEngine;
using UnityEngine.Scripting;
using Zenject;

namespace Sandbox._1._Simple_Request
{
    public class Main_SimpleRequest : MonoBehaviour
    {
        [Inject] private IMediator m_mediator;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                m_mediator.Publish(new LogMessage("HELLO"));
            if (Input.GetKeyDown(KeyCode.Alpha2))
                m_mediator.Publish(new LogMessage("A HELLO"));
            if (Input.GetKeyDown(KeyCode.Alpha3))
                m_mediator.Publish(new LogMessage("A B HELLO"));

            if (Input.GetKeyDown(KeyCode.Q)) A();
            if (Input.GetKeyDown(KeyCode.W)) B();
        }

        private async void A()
        {
            Debug.Log(await m_mediator.Send(new DivideRequest(10, 5)));
        }

        private async void B()
        {
            Debug.Log(await m_mediator.Send(new DivideRequest(10, 0)));
        }

        public class LoggerPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
            where TRequest : IRequest<TResponse>
        {
            public UniTask<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken ct)
            {
                Debug.Log($"HANDLING {request}");
                return next(ct);
            }
        }

        public class DividerPipeline : IPipelineBehavior<DivideRequest, float>
        {
            public UniTask<float> Handle(DivideRequest request, RequestHandlerDelegate<float> next, CancellationToken ct)
            {
                if (request.A == 0 || request.B == 0)
                {
                    Debug.Log("Nooo !!");
                }

                return next(ct);
            }
        }

        public record DivideRequest(int A, int B) : IRequest<float>;

        public class DivideHandler : IRequestHandler<DivideRequest, float>
        {
            public UniTask<float> Handle(DivideRequest request, CancellationToken ct)
            {
                return new UniTask<float>(request.A / (float)request.B);
            }
        }

        public record LogMessage(string Message) : INotification;

        [Preserve]
        public class MessageNotStartsWithANotificationBehavior : INotificationBehavior<LogMessage>
        {
            public async UniTask Handle(LogMessage notification, NotificationHandlerDelegate next, CancellationToken ct = default)
            {
                if (notification.Message.StartsWith("A"))
                {
                    Debug.Log("Starts with A");
                    return;
                }

                Debug.Log("A GOOD");
                await next();
            }
        }

        [Preserve]
        public class MessageWithNoBNotificationBehavior : INotificationBehavior<LogMessage>
        {
            public async UniTask Handle(LogMessage notification, NotificationHandlerDelegate next, CancellationToken ct)
            {
                if (notification.Message.Contains('B'))
                {
                    Debug.Log("Contains B");
                    return;
                }

                Debug.Log("B GOOD");
                await next();
            }
        }

        [Preserve]
        public class LogMessageHandler : INotificationHandler<LogMessage>
        {
            public UniTask Handle(LogMessage message, CancellationToken ct)
            {
                Debug.Log(message.Message);
                return UniTask.CompletedTask;
            }
        }
    }
}
using System;
using Cysharp.Threading.Tasks;
using UMediator;
using UnityEngine;
using UnityEngine.Scripting;

namespace Sandbox._1._Simple_Request
{
    public class Main_SimpleRequest : MonoBehaviour
    {
        private IMediator m_mediator;

        private void Awake()
        {
            m_mediator = new Mediator(HandlersScanner.ScanHandlers(AppDomain.CurrentDomain.GetAssemblies()), new ActivatorMediatorTypeFactory());
        }

        [ContextMenu("Sum")]
        private async void GetSum()
        {
            Debug.Log(Time.time);
            int result = await m_mediator.Send(new TestSumRequest(3, 2));
            Debug.Log(result);
            Debug.Log(Time.time);
        }

        [ContextMenu("Message")]
        private async void LogMessage()
        {
            Debug.Log(Time.time);
            await m_mediator.Send(new LogMessageCommand("MESSAGE"));
            Debug.Log(Time.time);
        }


        public record LogMessageCommand(string Message) : IRequest;

        public class LogMessageHandler : IRequestHandler<LogMessageCommand>
        {
            public async UniTask Handle(LogMessageCommand request)
            {
                Debug.Log("1 " + request.Message);
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                Debug.Log("2 " + request.Message);
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                Debug.Log("3 " + request.Message);
            }
        }

        public record TestSumRequest(int A, int B) : IRequest<int>;

        [Preserve]
        public class TestSumHandler : IRequestHandler<TestSumRequest, int>
        {
            public async UniTask<int> Handle(TestSumRequest request)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(1f));
                return request.A + request.B;
            }
        }
    }
}
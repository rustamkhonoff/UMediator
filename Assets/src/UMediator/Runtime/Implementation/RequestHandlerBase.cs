using Cysharp.Threading.Tasks;

namespace UMediator
{
    internal abstract class RequestHandlerBase
    {
        public abstract UniTask<object> Handle(object request, object handler);
    }
}
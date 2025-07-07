using Cysharp.Threading.Tasks;

namespace UMediator
{
    public interface IRequestHandler<in TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        UniTask<TResponse> Handle(TRequest request);
    }

    public interface IRequestHandler<in TRequest> where TRequest : IRequest
    {
        UniTask Handle(TRequest request);
    }
}
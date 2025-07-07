using Cysharp.Threading.Tasks;

namespace UMediator
{
    public interface ISender
    {
        UniTask<T> Send<T>(IRequest<T> request);
        UniTask Send<T>(T request) where T : IRequest;
    }
}
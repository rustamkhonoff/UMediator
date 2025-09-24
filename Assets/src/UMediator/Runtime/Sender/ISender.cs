using System.Threading;
using Cysharp.Threading.Tasks;

namespace UMediator
{
    public interface ISender
    {
        UniTask<T> Send<T>(IRequest<T> request, CancellationToken ct = default);
        UniTask Send<T>(T request, CancellationToken ct = default) where T : IRequest;
    }
}
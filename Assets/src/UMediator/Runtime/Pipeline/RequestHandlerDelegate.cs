using System.Threading;
using Cysharp.Threading.Tasks;

namespace UMediator.Pipeline
{
    public delegate UniTask<TResponse> RequestHandlerDelegate<TResponse>(CancellationToken ct);

    // Для void-запросов (без результата)
    public delegate UniTask RequestHandlerDelegate(CancellationToken ct);
}
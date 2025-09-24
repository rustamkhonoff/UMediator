using System.Threading;
using Cysharp.Threading.Tasks;

namespace UMediator
{
    public interface IPublisher
    {
        UniTask Publish<T>(T notification, CancellationToken ct = default) where T : INotification;
    }
}
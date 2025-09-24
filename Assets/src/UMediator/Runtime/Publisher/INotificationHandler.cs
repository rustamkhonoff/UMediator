using System.Threading;
using Cysharp.Threading.Tasks;

namespace UMediator
{
    public interface INotificationHandler<in TNotification> where TNotification : INotification
    {
        UniTask Handle(TNotification message,CancellationToken ct);
    }
}
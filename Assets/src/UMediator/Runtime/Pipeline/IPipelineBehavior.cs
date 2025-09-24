using System.Threading;
using Cysharp.Threading.Tasks;

namespace UMediator.Pipeline
{
    public interface IPipelineBehavior<in TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        UniTask<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken ct);
    }

    public interface IPipelineBehavior<in TRequest>
        where TRequest : IRequest
    {
        UniTask Handle(
            TRequest request,
            RequestHandlerDelegate next,
            CancellationToken ct);
    }

    public interface INotificationBehavior<in TNotification> where TNotification : INotification
    {
        UniTask Handle(TNotification notification, NotificationHandlerDelegate next, CancellationToken ct = default);
    }

    public delegate UniTask NotificationHandlerDelegate();
}
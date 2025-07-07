using Cysharp.Threading.Tasks;

namespace UMediator
{
    public interface IPublisher
    {
        UniTask Publish<T>(T notification) where T : INotification;
    }
}
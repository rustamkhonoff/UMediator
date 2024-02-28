namespace UMediator.Publisher
{
    public interface IPublisher
    {
        void Publish<T>(T notification) where T : INotification;
    }
}
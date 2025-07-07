namespace UMediator.Publisher
{
    public interface INotificationHandler<in TNotification> where TNotification : INotification
    {
        void Handle(TNotification message);
    }
}
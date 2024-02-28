namespace UMediator.Sender
{
    public interface ISender
    {
        T Send<T>(IRequest<T> request);
        void Send<T>(T request) where T : IRequest;
    }
}
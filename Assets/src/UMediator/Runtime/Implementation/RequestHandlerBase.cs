namespace UMediator.Implementation
{
    internal abstract class RequestHandlerBase
    {
        public abstract object Handle(object request, object handler);
    }
}
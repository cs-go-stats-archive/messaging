using CSGOStats.Infrastructure.Messaging.Payload;

namespace CSGOStats.Infrastructure.Messaging.Handling
{
    public interface IMessageRegistrar
    {
        void Register<T>(IMessageHandler<T> handler) 
            where T : class, IMessage;
    }
}
using System;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Extensions;
using CSGOStats.Infrastructure.Messaging.Payload;

namespace CSGOStats.Infrastructure.Messaging.Handling
{
    public abstract class BaseMessageHandler<T> : IMessageHandler
        where T : class, IMessage
    {
        public Type HandlingType => typeof(T);

        public abstract Task HandleAsync(T message);

        Task IMessageHandler.HandleAsync(object message) => HandleAsync(message.OfType<T>());
    }
}
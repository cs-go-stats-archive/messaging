using System;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Messaging.Payload;

namespace CSGOStats.Infrastructure.Messaging.Transport
{
    public interface IEventBus : IDisposable
    {
        Task PublishAsync(IMessage message);
    }
}
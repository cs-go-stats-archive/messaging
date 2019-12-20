using System;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Messaging.Extensions;
using CSGOStats.Infrastructure.Messaging.Handling;
using EasyNetQ;
using IMessage = CSGOStats.Infrastructure.Messaging.Payload.IMessage;

namespace CSGOStats.Infrastructure.Messaging.Transport
{
    public class RabbitMqEventBus : IEventBus, IMessageRegistrar
    {
        private readonly IBus _bus;

        public RabbitMqEventBus(RabbitMqConnectionConfiguration configuration)
        {
            configuration.NotNull(nameof(configuration));

            _bus = RabbitHutch.CreateBus(
                hostName: configuration.Host,
                hostPort: (ushort) configuration.Port,
                virtualHost: "/",
                username: configuration.Username,
                password: configuration.Password,
                requestedHeartbeat: (ushort) configuration.Heartbeat,
                registerServices: _ => { });
        }

        public void Dispose() => _bus.Dispose();

        public Task PublishAsync(IMessage message) => _bus.SendAsync(GetQueueName(message.GetType()), message);

        public void Register<T>(IMessageHandler<T> handler)
            where T : class, IMessage
        {
            _bus.SubscribeAsync<T>(Guid.NewGuid().ToString(), handler.HandleAsync, configuration =>
            {
                configuration.WithQueueName(GetQueueName(typeof(T)));
            });
        }

        private static string GetQueueName(Type type) => type.Assembly.GetName().Name;
    }
}
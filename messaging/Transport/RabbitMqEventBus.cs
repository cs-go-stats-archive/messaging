using System;
using System.Linq;
using System.Threading.Tasks;
using CSGOStats.Extensions.Validation;
using CSGOStats.Infrastructure.Messaging.Handling;
using EasyNetQ;
using EasyNetQ.FluentConfiguration;
using EasyNetQ.NonGeneric;
using Microsoft.Extensions.DependencyInjection;
using IMessage = CSGOStats.Infrastructure.Messaging.Payload.IMessage;

namespace CSGOStats.Infrastructure.Messaging.Transport
{
    public class RabbitMqEventBus : IEventBus, IMessageRegistrar
    {
        private readonly IBus _bus;
        private readonly IServiceProvider _serviceProvider;

        public RabbitMqEventBus(RabbitMqConnectionConfiguration configuration, IServiceProvider serviceProvider)
        {
            configuration.NotNull(nameof(configuration));
            _serviceProvider = serviceProvider.NotNull(nameof(serviceProvider));

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

        public Task PublishAsync(IMessage message) =>
            _bus.SendAsync(GetQueueName(message.GetType()), message);

        public void RegisterForType(Type type) =>
            _bus.SubscribeAsync(
                messageType: type,
                subscriptionId: Guid.NewGuid().ToString("D"),
                onMessage: HandleMessageAsync,
                configure: configuration => SubscriptionConfiguration(configuration, type));

        private Task HandleMessageAsync(object rawMessage)
        {
            var messageHandler = FindCorrespondingHandler(rawMessage);
            return messageHandler == null ? Task.CompletedTask : messageHandler.HandleAsync(rawMessage);
        }

        private IMessageHandler FindCorrespondingHandler(object message) =>
            _serviceProvider
                .GetServices<IHandler>()
                .Cast<IMessageHandler>()
                .SingleOrDefault(x => x.HandlingType == message.NotNull(nameof(message)).GetType());

        private static string GetQueueName(Type type) => type.Assembly.GetName().Name;

        private static void SubscriptionConfiguration(ISubscriptionConfiguration configuration, Type messageType) =>
            configuration.WithQueueName(GetQueueName(messageType));
    }
}
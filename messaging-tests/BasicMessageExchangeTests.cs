using System;
using System.Threading.Tasks;
using CSGOStats.Infrastructure.Messaging.Handling;
using CSGOStats.Infrastructure.Messaging.Tests.Model;
using CSGOStats.Infrastructure.Messaging.Tests.State;
using CSGOStats.Infrastructure.Messaging.Transport;
using FluentAssertions;
using Xunit;

namespace CSGOStats.Infrastructure.Messaging.Tests
{
    public class BasicMessageExchangeTests
    {
        [Fact]
        public async Task PublishSubscribeTestAsync()
        {
            using var bus = new RabbitMqEventBus(new RabbitMqConnectionConfiguration(
                "localhost",
                5672,
                "guest",
                "guest",
                1000));

            var handler = new TestHandler();
            bus.Register(handler);

            var random = new Random();
            var message = new TestMessage(
                id: Guid.NewGuid().ToString("D"),
                version: random.Next(),
                data: new TestData(
                    date: DateTime.UtcNow,
                    time: TimeSpan.FromMilliseconds(random.NextDouble() * random.Next())));
            await bus.PublishAsync(message);

            await Task.Delay(TimeSpan.FromSeconds(.5));

            SharedData.Message.Id.Should().Be(message.Id);
            SharedData.Message.Version.Should().Be(message.Version);
            SharedData.Message.Data.Date.Should().Be(message.Data.Date);
            SharedData.Message.Data.Time.Should().Be(message.Data.Time);
        }

    }

    public class TestHandler : IMessageHandler<TestMessage>
    {
        public Task HandleAsync(TestMessage message)
        {
            SharedData.Message = message;
            return Task.CompletedTask;
        }
    }
}
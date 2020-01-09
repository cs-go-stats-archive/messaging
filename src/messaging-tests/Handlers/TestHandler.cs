using System.Threading.Tasks;
using CSGOStats.Infrastructure.Messaging.Handling;
using CSGOStats.Infrastructure.Messaging.Tests.Model;
using CSGOStats.Infrastructure.Messaging.Tests.State;

namespace CSGOStats.Infrastructure.Messaging.Tests.Handlers
{
    public class TestHandler : BaseMessageHandler<TestMessage>
    {
        public override Task HandleAsync(TestMessage message)
        {
            SharedData.Message = message;
            return Task.CompletedTask;
        }
    }
}
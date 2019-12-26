using System.Threading.Tasks;
using CSGOStats.Infrastructure.Messaging.Handling;
using CSGOStats.Infrastructure.Messaging.Tests.Model;

namespace CSGOStats.Infrastructure.Messaging.Tests.Handlers
{
    public class AdvancedTestHandler : BaseMessageHandler<TestMessage>
    {
        public override Task HandleAsync(TestMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
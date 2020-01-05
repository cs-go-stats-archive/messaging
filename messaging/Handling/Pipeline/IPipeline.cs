using System.Collections.Generic;
using System.Threading.Tasks;

namespace CSGOStats.Infrastructure.Messaging.Handling.Pipeline
{
    public interface IPipeline
    {
        Task RunAsync(IReadOnlyCollection<IMessageHandler> messageHandlers, object rawMessage);
    }
}
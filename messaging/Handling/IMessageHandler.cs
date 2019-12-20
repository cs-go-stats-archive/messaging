using System.Threading.Tasks;
using CSGOStats.Infrastructure.Messaging.Payload;

namespace CSGOStats.Infrastructure.Messaging.Handling
{
    public interface IMessageHandler<in T> 
        where T : class, IMessage
    {
        Task HandleAsync(T message);
    }
}
using System;
using System.Threading.Tasks;

namespace CSGOStats.Infrastructure.Messaging.Handling
{
    public interface IHandler
    {
    }

    public interface IMessageHandler : IHandler
    {
        Type HandlingType { get; }

        Task HandleAsync(object message);
    }
}
using System;

namespace CSGOStats.Infrastructure.Messaging.Handling
{
    public interface IMessageRegistrar
    {
        void RegisterForType(Type type);
    }
}
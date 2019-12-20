using CSGOStats.Infrastructure.Messaging.Payload;

namespace CSGOStats.Infrastructure.Messaging.Tests.Model
{
    public class TestMessage : IMessage
    {
        public string Id { get; }

        public long Version { get; }

        public TestData Data { get; }

        public TestMessage(string id, long version, TestData data)
        {
            Id = id;
            Version = version;
            Data = data;
        }
    }
}
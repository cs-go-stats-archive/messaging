using CSGOStats.Extensions.Validation;

namespace CSGOStats.Infrastructure.Messaging.Config
{
    public class RetrySetting
    {
        public int RetryCount { get; }

        public RetrySetting(int retryCount)
        {
            RetryCount = retryCount.Positive(nameof(retryCount));
        }
    }
}
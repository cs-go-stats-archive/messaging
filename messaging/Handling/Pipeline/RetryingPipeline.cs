using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSGOStats.Extensions.Validation;
using CSGOStats.Infrastructure.Messaging.Config;

namespace CSGOStats.Infrastructure.Messaging.Handling.Pipeline
{
    public class RetryingPipeline : Pipeline
    {
        private readonly RetrySetting _retrySetting;

        public RetryingPipeline(IEnumerable<IPipe> pipes, RetrySetting retrySetting)
            : base(pipes)
        {
            _retrySetting = retrySetting.NotNull(nameof(retrySetting));
        }

        protected override async Task HandleAsync(IMessageHandler messageHandler, object rawMessage)
        {
            var exceptions = new Exception[_retrySetting.RetryCount];

            for (var @try = 0; @try < _retrySetting.RetryCount; @try++)
            {
                try
                {
                    await TryHandleAsync(messageHandler, rawMessage);
                    return;
                }
                catch (Exception e)
                {
                    exceptions[@try] = e;
                    await Task.Delay(TimeSpan.FromSeconds(.1));
                }
            }

            throw new AggregateException(exceptions);
        }

        private static Task TryHandleAsync(IMessageHandler messageHandler, object rawMessage) =>
            messageHandler.HandleAsync(rawMessage);
    }
}
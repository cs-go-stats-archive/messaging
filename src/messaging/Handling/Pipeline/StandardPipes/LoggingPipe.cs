using System;
using System.Threading.Tasks;
using CSGOStats.Extensions.Validation;
using Microsoft.Extensions.Logging;

namespace CSGOStats.Infrastructure.Messaging.Handling.Pipeline.StandardPipes
{
    public class LoggingPipe : IPipe
    {
        private readonly ILogger<LoggingPipe> _logger;

        public LoggingPipe(ILogger<LoggingPipe> logger)
        {
            _logger = logger.NotNull(nameof(logger));
        }

        public Task OnStartAwait(object rawMessage)
        {
            _logger.LogDebug($"Start handling message of type '{rawMessage.GetType()}'.");
            return Task.CompletedTask;
        }

        public Task OnExceptionAsync(object rawMessage, Exception exception)
        {
            _logger.LogError(exception, $"Exception occured during handling message of type '{rawMessage.GetType()}'.");
            LogComplete(rawMessage);
            return Task.CompletedTask;
        }

        public Task OnCompleteAwait(object rawMessage)
        {
            LogComplete(rawMessage);
            return Task.CompletedTask;
        }

        private void LogComplete(object rawMessage) =>
            _logger.LogDebug($"Complete handling message of type '{rawMessage.GetType()}'.");
    }
}
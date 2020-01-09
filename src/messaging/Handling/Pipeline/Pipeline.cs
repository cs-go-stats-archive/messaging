using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSGOStats.Extensions.Extensions;
using CSGOStats.Extensions.Validation;

namespace CSGOStats.Infrastructure.Messaging.Handling.Pipeline
{
    public class Pipeline : IPipeline
    {
        private readonly IReadOnlyCollection<IPipe> _pipes;

        public Pipeline(IEnumerable<IPipe> pipes)
        {
            _pipes = pipes.NotNull(nameof(pipes)).ToArrayFast();
        }

        public async Task RunAsync(IReadOnlyCollection<IMessageHandler> messageHandlers, object rawMessage)
        {
            await OnStartAwait(rawMessage);

            try
            {
                await HandleAsync(messageHandlers, rawMessage);
            }
            catch (Exception exception)
            {
                await OnExceptionAsync(rawMessage, exception);
                return;
            }

            await OnCompleteAwait(rawMessage);
        }

        protected virtual Task HandleAsync(IMessageHandler messageHandler, object rawMessage) =>
            messageHandler.HandleAsync(rawMessage);

        private async Task OnStartAwait(object rawMessage)
        {
            foreach (var pipe in _pipes)
            {
                await pipe.OnStartAwait(rawMessage);
            }
        }

        private async Task HandleAsync(IReadOnlyCollection<IMessageHandler> messageHandlers, object rawMessage)
        {
            foreach (var messageHandler in messageHandlers)
            {
                await HandleAsync(messageHandler, rawMessage);
            }
        }

        private async Task OnExceptionAsync(object rawMessage, Exception exception)
        {
            foreach (var pipe in _pipes)
            {
                await pipe.OnExceptionAsync(rawMessage, exception);
            }
        }

        private async Task OnCompleteAwait(object rawMessage)
        {
            foreach (var pipe in _pipes)
            {
                await pipe.OnCompleteAwait(rawMessage);
            }
        }
    }
}
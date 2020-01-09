using System;
using System.Threading.Tasks;

namespace CSGOStats.Infrastructure.Messaging.Handling.Pipeline
{
    public interface IPipe
    {
        Task OnStartAwait(object rawMessage);

        Task OnExceptionAsync(object rawMessage, Exception exception);
        
        Task OnCompleteAwait(object rawMessage);
    }
}
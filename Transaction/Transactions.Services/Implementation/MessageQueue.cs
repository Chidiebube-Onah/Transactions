using Serilog;
using Transactions.Logger;
using Transactions.Services.Interfaces;

namespace Transactions.Services.Implementation;

public class MessageQueue : IMessageQueue
{
    public async Task PublishAsync<T>(T @event, string queue)
    {
        await Task.FromResult(() =>
        {
            Log
                .ForContext(new PropertyBagEnricher().Add(queue,
                    @event, destructureObject: true)).Information(queue);
        });
    }
}
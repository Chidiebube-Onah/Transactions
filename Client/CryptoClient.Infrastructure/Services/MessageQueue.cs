using MassTransit;

namespace CryptoClient.Infrastructure.Services;

public class MessageQueue : IMessageQueue
{
    private readonly IBus _bus;

    private  Uri _uri; 
    public MessageQueue(IBus bus)
    {
        _bus = bus;

    }

    public async Task PublishAsync<T>(T @event, string queue)
    {
        _uri = new Uri($"rabbitmq://localhost/{queue}");

        var endPoint = await _bus.GetSendEndpoint(_uri);
        if (@event != null)
        {
            await endPoint.Send(@event, context =>
            {
                context.Durable = true; // Make message persistent
                context.SetRoutingKey(queue); // Set routing key to match queue name
                
            });
            await endPoint.Send(@event);
        }

    }

}

public interface UpdateTransaction
{
    public Guid ClientId { get; set; }
    public string WalletAddress { get; set; }
    public string Currency { get; set; }
    public string TransactionHash { get; set; }
    public DateTime Created { get; set; }
}


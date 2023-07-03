using CryptoClient.Infrastructure.Model;
using MassTransit;
using Microsoft.Extensions.Options;


namespace CryptoClient.Infrastructure.Services;

public class MessageQueue : IMessageQueue
{
    private readonly IOptions<MessageQueueConfig> _messagingConfig;
    private readonly IBus _bus;
    

    private  Uri _uri; 
    public MessageQueue(IOptions<MessageQueueConfig> messagingConfig, IBus bus)
    {
        _messagingConfig = messagingConfig;
        _bus = bus;
      ;
      
    }

    public async Task PublishAsync<T>(T @event, string queue)
    {
        _uri = new Uri($"{_messagingConfig.Value.Server}/{queue}");

        ISendEndpoint endPoint = await _bus.GetSendEndpoint(_uri);
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



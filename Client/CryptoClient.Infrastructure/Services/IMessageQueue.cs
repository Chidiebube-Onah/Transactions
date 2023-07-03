namespace CryptoClient.Infrastructure.Services;

public interface IMessageQueue
{
    Task PublishAsync<T>(T @event, string queue);
}
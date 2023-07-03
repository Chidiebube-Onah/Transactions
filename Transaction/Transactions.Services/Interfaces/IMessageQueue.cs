namespace Transactions.Services.Interfaces;

public interface IMessageQueue
{
    Task PublishAsync<T>(T @event, string queue);
}
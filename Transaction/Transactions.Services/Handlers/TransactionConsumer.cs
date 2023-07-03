using CryptoClient.Infrastructure.Model;
using MassTransit;
using Transactions.Model.Dtos.Request;
using Transactions.Services.Interfaces;

namespace Transactions.Services.Handlers;

public class TransactionConsumer : IConsumer<UpdateTransactionsCommandRequest>
{
    private readonly ITransactionsService _transactionsService;

    public TransactionConsumer(ITransactionsService transactionsService)
    {
        _transactionsService = transactionsService;
    }

    public async Task Consume(ConsumeContext<UpdateTransactionsCommandRequest> context)
    {
        await _transactionsService.UpdateTransaction(context.Message);
    }
}
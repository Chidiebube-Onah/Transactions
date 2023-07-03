using CryptoClient.Infrastructure.Model;

namespace CryptoClient.Infrastructure.Services;

public interface ITransactionService
{
    Task<UpdateTransactionsCommandRequest?> Update();
}
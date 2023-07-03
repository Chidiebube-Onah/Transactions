using CryptoClient.Infrastructure.Model;
using Transactions.Model.Dtos.Request;
using Transactions.Model.Dtos.Response;

namespace Transactions.Services.Interfaces;

public interface ITransactionsService
{
    Task<PagedResponse<TransactionResponse>> GetTransactions(TransactionRequestParams request);
    Task UpdateTransaction(UpdateTransactionsCommandRequest request);
}
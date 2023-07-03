using Transactions.Model.Dtos.Response;

namespace Transactions.Services.Interfaces;

public interface ICryptoApiClient
{
    Task<(TransactionResponse? response, bool isSuccessful)> GetTransactions(string transactionHash, string walletAddress, string currencyType);
}
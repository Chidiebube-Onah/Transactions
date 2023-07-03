using Newtonsoft.Json;
using Serilog;
using Transactions.Model.Dtos.Response;
using Transactions.Services.Interfaces;

namespace Transactions.Services.Implementation;

public class CryptoApiClient : ICryptoApiClient
{
    private readonly ILogger _logger;
    private readonly HttpClient _httpClient;

    
    public CryptoApiClient(ILogger logger, HttpClient httpClient)
    {
        _logger = logger;
        _httpClient = httpClient;
        
    }

    public async Task<(TransactionResponse? response, bool isSuccessful)> GetTransactions(string transactionHash, string walletAddress, string currencyType)
    {
        // Implement the logic to make HTTP requests or interact with the crypto API client library
        // Query the crypto API and retrieve transactions based on the wallet address and currency type
        // Return the retrieved transactions
          
        // Example implementation:
       
        var transactionsTemplate = $"?transactionhash={transactionHash}&walletaddress={walletAddress}&currency={currencyType}";

        try
        {
            var response = await _httpClient.GetAsync(transactionsTemplate);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var transaction = JsonConvert.DeserializeObject<TransactionResponse>(content);

            return (transaction, true);
        }
        catch (Exception ex)
        {
           
            Log.Error($"An error occurred while retrieving transactions from the crypto API: {ex.Message}");
            return (null, false);

        }
    }

}
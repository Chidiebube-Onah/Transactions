namespace CryptoClient.Infrastructure.Model;

public class UpdateTransactionsCommandRequest
{
    public string ClientId { get; set; }
    public string WalletAddress { get; set; }
    public string Currency { get; set; }
    public string TransactionHash { get; set; }
}
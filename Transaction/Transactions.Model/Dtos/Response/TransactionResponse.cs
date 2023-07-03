namespace Transactions.Model.Dtos.Response;

public class TransactionResponse
{
    public string TransactionHash { get; set; }
    public string TransactionStatus { get; set; }
    public string TransactionType { get; set; }
    public string TransactionHashUrl { get; set; }
    public string ToAddress { get; set; }
    public string FromAddress { get; set; }
    public string ToAddressUrl { get; set; }
    public string FromAddressUrl { get; set; }
    public decimal Amount { get; set; }
    public string Network { get; set; }
    public string Currency { get; set; }
    public long TransactionTimeStamp { get; set; }
    public string Reference { get; set; }
    public DateTime CreatedAt { get; set; }
}
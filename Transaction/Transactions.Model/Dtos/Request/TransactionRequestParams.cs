using System.ComponentModel.DataAnnotations;

namespace Transactions.Model.Dtos.Request;

public class TransactionRequestParams : RequestParameters
{
    public string? Currency { get; set; }
    public string? Network { get; set; }
    public string? Status { get; set; }
    public string? TransactionHash { get; set; }
    public string? WalletAddress { get; set; }
    public string? TransactionType { get; set; }
    public DateTime? StartTimeStamp { get; set; }
    public DateTime? EndTimeStamp { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Transactions.Model.Enums;

namespace Transactions.Model.Entities;

public class TransactionUpdateRequest
{

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string RequestId { get; set; }
    public string ClientId { get; set; }
    public RequestStatus Status { get; set; }
    public string TransactionHash { get; set; }
    public string WalletAddress { get; set; }
    public string Currency { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
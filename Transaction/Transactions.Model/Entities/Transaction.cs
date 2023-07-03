using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Transactions.Model.Entities
{
    public class Transaction
    {
        [Key]
        public string Id { get; set; }
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
        public DateTime TransactionTimeStamp { get; set; }
        public string Reference { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

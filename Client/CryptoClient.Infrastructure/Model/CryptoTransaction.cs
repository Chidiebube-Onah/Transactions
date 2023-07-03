using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoClient.Infrastructure.Model
{
    public class CryptoTransaction
    {
        public string TransactionStatus { get; set; }
        public string TransactionType { get; set; }
        public string TransactionHash { get; set; }
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

   
        public class CryptoClientRequest 
        {

            [Required]
            public string Currency { get; set; }

            [Required]
            public string WalletAddress { get; set; }

            [Required]
            public string TransactionHash { get; set; }
         
        }

        public class PagedList<T> : List<T>
        {
            public MetaData MetaData { get; set; }

            public PagedList(List<T> items, long count, int pageNumber, int pageSize)
            {
                MetaData = new MetaData
                {
                    TotalCount = count,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = (int) Math.Ceiling(count / (double) pageSize)
                };

                AddRange(items);
            }
        }

        public class MetaData
        {
            public int CurrentPage { get; set; }
            public int TotalPages { get; set; }
            public int PageSize { get; set; }
            public long TotalCount { get; set; }

            public bool HasPrevious => CurrentPage > 1;
            public bool HasNext => CurrentPage < TotalPages;
        }

    public abstract class RequestParameters
        {
            const int maxPageSize = 50;
            public int PageNumber { get; set; } = 1;

            private int _pageSize = 10;
            public int PageSize
            {
                get
                {
                    return _pageSize;
                }
                set
                {
                    _pageSize = (value > maxPageSize) ? maxPageSize : value;
                }
            }


        }
}

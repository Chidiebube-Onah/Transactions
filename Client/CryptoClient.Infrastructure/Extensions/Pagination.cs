using CryptoClient.Infrastructure.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoClient.Infrastructure.Extensions
{
    public static class Pagination
    {
        public static PagedList<T> GetPagedItems<T>(this IEnumerable<T> query, RequestParameters parameters)
        {
            var skip = (parameters.PageNumber - 1) * parameters.PageSize;

            var items = query.Skip(skip).Take(parameters.PageSize).ToList();
            return new PagedList<T>(items, query.Count(), parameters.PageNumber, parameters.PageSize);
        }
    }
}

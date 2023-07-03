using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Transactions.Data.Extensions.Utilities;
using Transactions.Model.Dtos.Request;
using Transactions.Model.Dtos.Response;

namespace Transactions.Data.Extensions
{
    public static class RepositoryExtensions
    {
        public static async Task RunPendingMigrationsAsync<T>(this T db) where T : DbContext
        {
            List<string> pendingMigrations = (await db.Database.GetPendingMigrationsAsync()).ToList();

            if (pendingMigrations.Any())
            {
                IMigrator migrator = db.Database.GetService<IMigrator>();

                foreach (string targetMigration in pendingMigrations)
                    await migrator.MigrateAsync(targetMigration);
            }

            await Task.CompletedTask;
        }
        public static IQueryable<T> Sort<T>(this IQueryable<T> query, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return query;

            string orderQuery = OrderQueryBuilder.CreateOrderQuery<T>(orderByQueryString);

            if (string.IsNullOrWhiteSpace(orderQuery))
                return query;

            return query.OrderBy(orderQuery);
        }

        public static async Task<PagedList<T>> GetPagedItems<T>(this IQueryable<T> query, RequestParameters parameters,Expression<Func<T, bool>> searchExpression = null)
        {
            int skip = (parameters.PageNumber - 1) * parameters.PageSize;
            if (searchExpression != null)
                query = query.Where(searchExpression);

            if (!string.IsNullOrWhiteSpace(parameters.OrderBy)) 
                query = query.Sort(parameters.OrderBy);
            
            List<T> items = await query.Skip(skip).Take(parameters.PageSize).ToListAsync();
            return new PagedList<T>(items, await query.CountAsync(), parameters.PageNumber, parameters.PageSize);
        }

        public static PagedList<T> GetPagedItems<T>(this IEnumerable<T> query, RequestParameters parameters)
        {
            int skip = (parameters.PageNumber - 1) * parameters.PageSize;
            
            List<T> items = query.Skip(skip).Take(parameters.PageSize).ToList();
            return new PagedList<T>(items, query.Count(), parameters.PageNumber, parameters.PageSize);
        }
    }

}

using Microsoft.Extensions.DependencyInjection;
using Transactions.Data.Context;
using Transactions.Data.Implementation;
using Transactions.Data.Interfaces;
using Transactions.Services.Implementation;
using Transactions.Services.Interfaces;

namespace Transactions.Services.Extensions
{
    public static class MiddlewareExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork<TransactionsDbContext>>();
            services.AddTransient<IServiceFactory, ServiceFactory>();
            services.AddScoped<ITransactionsService, TransactionsService>();
            services.AddScoped<IMessageQueue, MessageQueue>();
        }
    }
}

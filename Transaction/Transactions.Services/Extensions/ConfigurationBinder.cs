using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Transactions.Services.Extensions
{
    public static class ConfigurationBinder
    {
        public static IServiceCollection BindConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
           

            // services.AddSingleton(jwt);
            // services.AddSingleton(appConstants);
            // services.AddSingleton(remitaConfiguration);
            // services.AddSingleton(schoolAccount);
            // services.AddSingleton(teneceAccount);
            // services.AddSingleton(settings);


            return services;
        }
    }
}

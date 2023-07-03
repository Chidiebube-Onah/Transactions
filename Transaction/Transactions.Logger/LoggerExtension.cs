using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Routing;

namespace Transactions.Logger
{
    public static class LoggerExtension
    {
        public static string? GetMetricsCurrentResourceName(this HttpContext httpContext)
        {
            if (httpContext == null)
                throw new ArgumentNullException(nameof(httpContext));

            Endpoint? endpoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;

           return endpoint?.Metadata.GetMetadata<EndpointNameMetadata>()?.EndpointName;

        }


        public static string? GetUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static string? GetUserId(this ClaimsPrincipal user)
        {
            return user.FindFirst("Id")?.Value;
        }

        public static IEnumerable<string> GetRoles(this ClaimsPrincipal user)
        {
            return user.Claims.Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value);
        }

        public static string? GetImpersonatorId(this ClaimsPrincipal user)
        {
            return user.FindFirst("ImpersonatorId")?.Value;
        }

        public static string? GetImpersonatorUsername(this ClaimsPrincipal user)
        {
            return user.FindFirst("ImpersonatorUsername")?.Value;
        }
    }
}

using Microsoft.AspNetCore.Http;
using Serilog;

namespace Transactions.Logger
{
    public static class LogEnricher
    {
        /// <summary>
        /// Enriches the HTTP request log with additional data via the Diagnostic Context
        /// </summary>
        /// <param name="diagnosticContext">The Serilog diagnostic context</param>
        /// <param name="httpContext">The current HTTP Context</param>
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            diagnosticContext.Set("ClientIP", httpContext.Connection.RemoteIpAddress.ToString());
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());

            string? resource = httpContext.GetMetricsCurrentResourceName();
            string? userId = httpContext.User?.GetUserId();
            string? impersonatorId = httpContext.User?.GetImpersonatorId();
            string? username = httpContext.User?.GetUsername();
            string? impersonatorUsername = httpContext.User?.GetImpersonatorUsername();
            IEnumerable<string> roles = httpContext.User?.GetRoles() ?? Array.Empty<string>();


            if (!string.IsNullOrWhiteSpace(resource))
            {
                diagnosticContext.Set("Resource", resource);
            }

            if (!string.IsNullOrWhiteSpace(userId))
            {
                diagnosticContext.Set("UserId", userId);

            }

            if (!string.IsNullOrWhiteSpace(impersonatorId))
            {
                diagnosticContext.Set("ImpersonatorId", impersonatorId);

            }

            if (!string.IsNullOrWhiteSpace(username))
            {
                diagnosticContext.Set("Username", username);

            }

            if (!string.IsNullOrWhiteSpace(impersonatorUsername))
            {
                diagnosticContext.Set("ImpersonatorUsername", impersonatorUsername);

            }

            if (roles.Any())
            {
                diagnosticContext.Set("Roles", roles);

            }

        }
    }
}


using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using EStoreX.Core.ServiceContracts.Account;
using System.Threading.Tasks;
using EStoreX.Core.Helper;

namespace E_StoreX.API.Middleware
{
    /// <summary>
    /// Middleware to validate API key from incoming HTTP requests.
    /// </summary>
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string API_KEY_HEADER_NAME = "X-API-KEY";
        private readonly List<string> allowedStaticPaths = new List<string>
        {
            "/reset-password", "/password-reset-success", "/password-reset-failed",
            "/invalid-reset-link", "/index",
            //"/api/account/external-login",
            "/api/v1/account/external-login-callback",
            "/signin-google"
        };
        private readonly List<string> allowedStaticExtensions = new List<string>
        {
            ".html", ".js", ".css", ".png", ".jpg", ".jpeg", ".svg"
        };

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiKeyMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the pipeline.</param>
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invokes the middleware to check for a valid API key.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <param name="apiClientService">api client</param>
        public async Task InvokeAsync(HttpContext context, IApiClientService apiClientService)
        {

            var path = context.Request.Path.Value?.ToLower();

            if (allowedStaticPaths.Any(p => path.StartsWith(p)) ||
                allowedStaticExtensions.Any(ext => path.EndsWith(ext)) ||
                context.Request.Method == HttpMethods.Options ||
                context.Request.Path.StartsWithSegments("/api/v2/frontend/reset") ||
                context.Request.Path.StartsWithSegments("/favicon.ico") ||
                path.Contains("webhook"))
            {
                await _next(context);
                return;
            }

            if (path.Contains("/api/") && path.Contains("/account/external-login"))
            {
                if (context.Request.Query.ContainsKey(API_KEY_HEADER_NAME))
                {
                    await _next(context);
                    return;
                }
            }

            if (!context.Request.Headers.TryGetValue(API_KEY_HEADER_NAME, out var extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(ApiResponseFactory.Unauthorized("API Key is missing."));
                return;
            }

            if (string.IsNullOrWhiteSpace(extractedApiKey))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(ApiResponseFactory.BadRequest("API Key cannot be empty."));
                return;
            }

            var client = await apiClientService.GetByApiKeyAsync(extractedApiKey!);

            if (client == null)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(ApiResponseFactory.Forbidden("Invalid API Key.", new List<string>
                {
                    "Invalid API Key."
                }));
                return;
            }

            if (!client.IsActive)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                await context.Response.WriteAsJsonAsync(ApiResponseFactory.Forbidden( "API Key is deactivated. Please contact support to reactivate it.", 
                new List<string>
                {
                    "API Key is deactivated. Please contact support to reactivate it."
                }));
                return;
            }

            await _next(context);
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}

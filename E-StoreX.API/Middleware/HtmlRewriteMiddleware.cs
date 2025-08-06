using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace E_StoreX.API.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    /// <summary>
    /// html rewrite middleware
    /// </summary>
    public class HtmlRewriteMiddleware
    {
        private readonly RequestDelegate _next;
        /// <summary>
        /// html rewrite middleware constructor
        /// </summary>
        /// <param name="next">call next middleware</param>
        public HtmlRewriteMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// html rewrite middleware invoke method
        /// </summary>
        /// <param name="context">current context</param>
        /// <returns>call next</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path == "/reset-password")
            {
                context.Request.Path = "/reset-password.html";
            }
            else if (path == "/password-reset-success")
            {
                context.Request.Path = "/password-reset-success.html";
            }
            else if (path == "/password-reset-failed")
            {
                context.Request.Path = "/password-reset-failed.html";
            }
            else if (path == "/invalid-reset-link")
            {
                context.Request.Path = "/invalid-reset-link.html";
            }

            await _next(context);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    /// <summary>
    /// html rewrite middleware extensions
    /// </summary>
    public static class HtmlRewriteMiddlewareExtensions
    {
        /// <summary>
        /// html rewrite middleware extension method
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseHtmlRewriteMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HtmlRewriteMiddleware>();
        }
    }
}

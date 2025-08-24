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
            context.Request.Path = path switch
            {
                "/reset-password" => "/reset-password.html",
                "/password-reset-success" => "/password-reset-success.html",
                "/password-reset-failed" => "/password-reset-failed.html",
                "/invalid-reset-link" => "/invalid-reset-link.html",
                "/email-confirmed" => "/email-confirmed.html",
                "/email-confirm-failed" => "/email-confirm-failed.html",
                _ => context.Request.Path 
            };
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

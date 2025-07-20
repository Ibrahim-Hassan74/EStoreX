using EStoreX.Infrastructure;
using EStoreX.Core;
using Microsoft.Extensions.FileProviders;
using E_StoreX.API.Middleware;
using E_StoreX.API.Helper;
using System.Threading.RateLimiting;
using EStoreX.Core.Domain.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddMemoryCache();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        policybuilder => policybuilder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
    options.AddPolicy("CORSPolicy", // Use this policy in your controllers not use it now allow all is used
        policybuilder => policybuilder.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>()!)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
});


builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: ip,
            factory: _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 8, 
                Window = TimeSpan.FromSeconds(30),
                AutoReplenishment = true,
                QueueLimit = 0
            });
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        var response = new ApiExceptions(StatusCodes.Status429TooManyRequests, "Too Many Requests. Please try again later.");
        await context.HttpContext.Response.WriteAsJsonAsync(response);
    };
});

builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSetting"));


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(
        System.IO.Path.Combine(System.AppContext.BaseDirectory, "E-StoreX.API.xml"),
        includeControllerXmlComments: true
    );
});

builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
);

builder.Services.ConfigureInfrastructure(builder.Configuration);

builder.Services.ConfigureCore(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();

app.UseCors("AllowAllOrigins");

app.UseRateLimiter();

app.UseExceptionHandlingMiddleware();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

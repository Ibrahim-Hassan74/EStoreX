using E_StoreX.API.Middleware;
using EStoreX.API.Filters;
using EStoreX.Core;
using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.Domain.Options;
using EStoreX.Core.Helper;
using EStoreX.Infrastructure;
using EStoreX.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


var configPath = Path.Combine(builder.Environment.ContentRootPath, "Configurations");
builder.Configuration
    .AddJsonFile(Path.Combine(configPath, "appsettings.json"), optional: false, reloadOnChange: true)
    .AddJsonFile(Path.Combine(configPath, $"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true);

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
                PermitLimit = 30,
                Window = TimeSpan.FromSeconds(10),
                AutoReplenishment = true,
                QueueLimit = 0
            });
    });

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";

        var response = ApiResponseFactory.TooManyRequests("Too Many Requests. Please try again later.");
        await context.HttpContext.Response.WriteAsJsonAsync(response);
    };
});

builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireDigit = true;
})
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

builder.Services.AddScoped<AccountValidationFilter>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(x => x.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage)
            .ToList();

        var response = ApiResponseFactory.BadRequest("Validation failed.", errors);

        return new BadRequestObjectResult(response);
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
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityDefinition("X-API-KEY", new OpenApiSecurityScheme
    {
        Description = "API Key needed to access the endpoints.",
        Name = "X-API-KEY",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] {}
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "X-API-KEY"
                        }
                    },
                    new string[] {}
                }
            });
});

builder.Services.AddSingleton<IFileProvider>(
    new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
);

builder.Services.ConfigureInfrastructure(builder.Configuration);

builder.Services.ConfigureCore(builder.Configuration);

builder.Services.AddHttpClient();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseApiKeyMiddleware();

app.UseCors("AllowAllOrigins");

app.UseRateLimiter();

app.UseExceptionHandlingMiddleware();

app.UseHtmlRewriteMiddleware();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

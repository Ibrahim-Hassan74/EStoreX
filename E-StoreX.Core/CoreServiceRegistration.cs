using EStoreX.Core.Domain.Options;
using EStoreX.Core.ServiceContracts;
using EStoreX.Core.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ServiceContracts;
using System.Text;

namespace EStoreX.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection ConfigureCore(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICategoriesService, CategoriesService>();

            services.AddSingleton<IImageService, ImageService>();

            services.AddScoped<IProductsService, ProductsService>();

            services.AddScoped<IBasketService, BasketService>();

            services.AddScoped<IEmailSenderService, EmailSenderService>();

            services.AddTransient<IJwtService, JwtService>();

            services.AddScoped<IAuthenticationService, AuthenticationService>();

            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<IOrderService, OrderService>();

            // JWT
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    //ClockSkew = TimeSpan.Zero, // Prevents the default 5-minute clock drift tolerance when validating token expiration
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudiences = configuration.GetSection("Jwt:Audiences").Get<List<string>>(),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            });


            services.Configure<StripeSettings>(configuration.GetSection("StripeSetting"));

            return services;
        }
    }
}

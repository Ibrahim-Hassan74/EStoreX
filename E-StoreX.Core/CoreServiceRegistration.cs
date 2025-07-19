using EStoreX.Core.ServiceContracts;
using EStoreX.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EStoreX.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection ConfigureCore(this IServiceCollection services)
        {

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<ICategoriesService, CategoriesService>();

            services.AddSingleton<IImageService, ImageService>();

            services.AddScoped<IProductsService, ProductsService>();

            services.AddScoped<IBasketService, BasketService>();

            return services;
        }
    }
}

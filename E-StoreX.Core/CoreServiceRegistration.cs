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

            return services;
        }
    }
}

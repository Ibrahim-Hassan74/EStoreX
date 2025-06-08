using EStoreX.Core.RepositoryContracts;
using EStoreX.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace EStoreX.Infrastructure
{
    public static class InfrastructureRegisteration
    {
        public static IServiceCollection InfrastructureConfiguration(IServiceCollection services)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            return services;
        }
    }
}

using Domain.Entities.Common;
using EStoreX.Infrastructure.Repository.Common;
using EStoreX.Core.RepositoryContracts.Account;
using EStoreX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EStoreX.Infrastructure.Repository.Account
{
    /// <summary>
    /// Repository for accessing and managing API clients.
    /// </summary>
    public class ApiClientRepository : GenericRepository<ApiClient>, IApiClientRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor for ApiClientRepository.
        /// </summary>
        /// <param name="context">The application's DbContext.</param>
        public ApiClientRepository(ApplicationDbContext context) : base(context) 
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<bool> ApiKeyExistsAsync(string apiKey)
        {
            return await _context.ApiClients.AnyAsync(c => c.ApiKey == apiKey);
        }

        /// <inheritdoc />
        public async Task<ApiClient?> GetByApiKeyAsync(string apiKey)
        {
            return await _context.ApiClients
                .FirstOrDefaultAsync(c => c.ApiKey == apiKey);
        }
    }
}

using EStoreX.Core.Domain.Entities;
using EStoreX.Core.RepositoryContracts;
using EStoreX.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace EStoreX.Infrastructure.Repository
{
    /// <summary>
    /// Repository for accessing and managing API clients.
    /// </summary>
    public class ApiClientRepository : IApiClientRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Constructor for ApiClientRepository.
        /// </summary>
        /// <param name="context">The application's DbContext.</param>
        public ApiClientRepository(ApplicationDbContext context)
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

        /// <inheritdoc />
        public async Task AddAsync(ApiClient client)
        {
            await _context.ApiClients.AddAsync(client);
        }
    }
}

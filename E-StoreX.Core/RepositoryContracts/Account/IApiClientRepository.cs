using Domain.Entities.Common;
using EStoreX.Core.RepositoryContracts.Common;

namespace EStoreX.Core.RepositoryContracts.Account
{
    /// <summary>
    /// Interface for managing API clients in the data store.
    /// </summary>
    public interface IApiClientRepository : IGenericRepository<ApiClient>
    {
        /// <summary>
        /// Checks if an API key already exists.
        /// </summary>
        /// <param name="apiKey">The API key to check.</param>
        /// <returns>True if it exists, false otherwise.</returns>
        Task<bool> ApiKeyExistsAsync(string apiKey);

        /// <summary>
        /// Gets a client by its API key.
        /// </summary>
        /// <param name="apiKey">The API key of the client.</param>
        /// <returns>The matching ApiClient, or null if not found.</returns>
        Task<ApiClient?> GetByApiKeyAsync(string apiKey);
    }
}

using Domain.Entities.Common;

namespace EStoreX.Core.RepositoryContracts.Account
{
    /// <summary>
    /// Interface for managing API clients in the data store.
    /// </summary>
    public interface IApiClientRepository
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

        /// <summary>
        /// Adds a new API client to the data store.
        /// </summary>
        /// <param name="client">The ApiClient to add.</param>
        Task AddAsync(ApiClient client);
    }
}

using EStoreX.Core.Domain.Entities;

namespace ServiceContracts
{
    public interface IApiClientService
    {
        /// <summary>
        /// Creates a new API client and returns the result.
        /// </summary>
        /// <param name="clientName">The name of the client application.</param>
        /// <returns>ApiClient instance containing the generated API key.</returns>
        Task<ApiClient> CreateClientAsync(string clientName);

        /// <summary>
        /// Gets a client by API key.
        /// </summary>
        /// <param name="apiKey">The API key to search for.</param>
        /// <returns>ApiClient or null.</returns>
        Task<ApiClient?> GetByApiKeyAsync(string apiKey);
    }

}

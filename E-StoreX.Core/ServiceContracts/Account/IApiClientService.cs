using Domain.Entities.Common;
using EStoreX.Core.DTO.Common;

namespace EStoreX.Core.ServiceContracts.Account
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
        /// <summary>
        /// Activates a client account.
        /// </summary>
        /// <returns>
        /// True if the client was successfully activated, otherwise false.
        /// </returns>
        Task<bool> ActiveClientAsync(Guid clientId);
        /// <summary>
        /// Deactivates an API client by its identifier.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client to deactivate.</param>
        /// <returns>
        /// A task that resolves to <c>true</c> if the client was found and deactivated;
        /// otherwise <c>false</c> (e.g., when the client does not exist).
        /// </returns>
        Task<bool> DeActivateClientAsync(Guid clientId);

        /// <summary>
        /// Retrieves all API clients.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains an <see cref="IEnumerable{ApiClient}"/> of all clients.</returns>
        Task<IEnumerable<ApiClient>> GetClientsAsync();
        /// <summary>
        /// Retrieves a specific API client by its unique identifier.
        /// </summary>
        /// <param name="clientId">The unique identifier of the API client.</param>
        /// <returns>A task that represents the asynchronous operation. 
        /// The task result contains the <see cref="ApiClient"/> if found; otherwise, <c>null</c>.</returns>
        Task<ApiClient?> GetClientAsync(Guid clientId);
        /// <summary>
        /// Removes an API client permanently from the system.
        /// </summary>
        /// <param name="clientId">Unique identifier of the client to remove.</param>
        /// <returns>
        /// A task that represents the asynchronous operation. 
        /// The task result contains <c>true</c> if the client was successfully removed; 
        /// otherwise, <c>false</c>.
        /// </returns>
        Task<bool> RemoveClientAsync(Guid clientId);
        /// <summary>
        /// Updates an existing API client.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client to update.</param>
        /// <param name="request">The updated client data.</param>
        /// <returns>True if update succeeded, false otherwise.</returns>
        Task<bool> UpdateClientAsync(Guid clientId, UpdateClientRequest request);
        /// <summary>
        /// Rotates (regenerates) the API key for a specific client.
        /// </summary>
        /// <param name="clientId">The ID of the client.</param>
        /// <returns>The updated client with the new API key.</returns>
        Task<ApiClient?> RotateApiKeyAsync(Guid clientId);
    }

}

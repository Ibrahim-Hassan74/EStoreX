using Asp.Versioning;
using Domain.Entities.Common;
using EStoreX.Core.DTO.Account.Requests;
using EStoreX.Core.DTO.Account.Responses;
using EStoreX.Core.DTO.Common;
using EStoreX.Core.Helper;
using EStoreX.Core.ServiceContracts.Account;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// api clients controller  
    /// </summary>
    /// <remarks>This controller inherits from <see cref="AdminControllerBase"/> and is intended for use in
    /// administrative scenarios where order-related operations are required. It serves as a base for implementing
    /// actions related to order management.</remarks>
    [ApiVersion(2.0)]
    public class ApiClientsController : AdminControllerBase
    {
        private readonly IApiClientService _clientService;
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiClientsController"/> class with the specified API client
        /// service.
        /// </summary>
        /// <remarks>— This constructor is typically used by dependency injection to provide the required
        /// service for API client management.</remarks>
        /// <param name="clientService">The service used to manage API client data and operations. Cannot be <c>null</c>.</param>
        public ApiClientsController(IApiClientService clientService)
        {
            _clientService = clientService;
        }

        /// <summary>
        /// Registers a new API client and returns its API key.
        /// </summary>
        /// <param name="request">Client name info.</param>
        /// <returns>Client name and API key.</returns>
        [HttpPost("register")]
        public async Task<ActionResult<RegisterApiClientResponse>> RegisterClient([FromBody] RegisterApiClientRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ClientName))
                return BadRequest(ApiResponseFactory.BadRequest("Client name is required."));

            var client = await _clientService.CreateClientAsync(request.ClientName);

            return Ok(new RegisterApiClientResponse
            {
                ClientId = client.Id,
                ClientName = client.ClientName,
                ApiKey = client.ApiKey
            });
        }

        /// <summary>
        /// Retrieves client details by its API key.
        /// </summary>
        /// <param name="apiKey">The API key used to identify the client.</param>
        /// <returns>
        /// 200 OK: Returns the client details if the API key is valid.  
        /// 404 Not Found: If no client is associated with the provided API key.  
        /// </returns>
        [HttpGet("client/{apiKey}")]
        public async Task<IActionResult> GetClientByKey(string apiKey)
        {
            var res = await _clientService.GetByApiKeyAsync(apiKey);
            return Ok(res);
        }
        /// <summary>
        /// Retrieves all registered API clients.
        /// </summary>
        /// <returns>A list of <see cref="ApiClient"/> entities.</returns>
        [HttpGet("all")]
        public async Task<IActionResult> GetAllClients()
        {
            var res = await _clientService.GetClientsAsync();
            return Ok(res);
        }

        /// <summary>
        /// Activates an API client by its unique identifier.
        /// </summary>
        /// <param name="clientId">The unique identifier of the API client to activate.</param>
        /// <returns>200 OK if activated successfully; 404 if the client is not found.</returns>
        [HttpPut("activate/{clientId}")]
        public async Task<ActionResult<ApiResponse>> ActiveClient(Guid clientId)
        {
            var result = await _clientService.ActiveClientAsync(clientId);
            if (!result) return ApiResponseFactory.NotFound("Client not found.");
            return ApiResponseFactory.Success("Client activated successfully.");
        }

        /// <summary>
        /// Deactivates an API client by its unique identifier.
        /// </summary>
        /// <param name="clientId">The unique identifier of the API client to deactivate.</param>
        /// <returns>200 OK if deactivated successfully; 404 if the client is not found.</returns>
        [HttpPut("deactivate/{clientId}")]
        public async Task<ActionResult<ApiResponse>> DeActivateClient(Guid clientId)
        {
            var result = await _clientService.DeActivateClientAsync(clientId);
            if (!result) return ApiResponseFactory.NotFound("Client not found.");
            return ApiResponseFactory.Success("Client deactivated successfully.");
        }

        /// <summary>
        /// Retrieves a specific API client by its unique identifier.
        /// </summary>
        /// <param name="clientId">The unique identifier of the API client.</param>
        /// <returns>200 OK with the <see cref="ApiClient"/> data if found; 404 if not found.</returns>
        [HttpGet("{clientId}")]
        public async Task<IActionResult> GetClient(Guid clientId)
        {
            var client = await _clientService.GetClientAsync(clientId);
            if (client == null) return NotFound(ApiResponseFactory.NotFound("Client not found."));
            return Ok(client);
        }
        /// <summary>
        /// Removes an API client by its unique identifier.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client to remove.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the outcome of the operation.  
        /// Returns <c>200 OK</c> if the client was removed successfully,  
        /// or <c>404 Not Found</c> if the client does not exist.
        /// </returns>
        [HttpDelete("{clientId:guid}")]
        public async Task<IActionResult> RemoveClient(Guid clientId)
        {
            var result = await _clientService.RemoveClientAsync(clientId);
            if (!result) return NotFound(ApiResponseFactory.NotFound("Client not found."));
            return Ok(ApiResponseFactory.Success("Client removed successfully."));
        }
        /// <summary>
        /// Updates an API client by its unique identifier.
        /// </summary>
        /// <param name="clientId">The unique identifier of the client.</param>
        /// <param name="request">The updated client data.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the outcome of the update operation.  
        /// Returns <c>200 OK</c> if updated successfully,  
        /// or <c>404 Not Found</c> if the client does not exist.
        /// </returns>
        [HttpPut("{clientId:guid}")]
        public async Task<IActionResult> UpdateClient(Guid clientId, [FromBody] UpdateClientRequest request)
        {
            var result = await _clientService.UpdateClientAsync(clientId, request);

            if (!result)
                return NotFound(ApiResponseFactory.NotFound("Client not found."));

            return Ok(ApiResponseFactory.Success("Client updated successfully."));
        }

        /// <summary>
        /// Rotates the API key of a specific client.
        /// </summary>
        /// <param name="clientId">The ID of the client.</param>
        /// <returns>Newly generated API key.</returns>
        [HttpPost("{clientId}/rotate-api-key")]
        public async Task<IActionResult> RotateApiKey(Guid clientId)
        {
            var client = await _clientService.RotateApiKeyAsync(clientId);
            if (client == null)
                return NotFound(ApiResponseFactory.NotFound("Client not found."));

            return Ok(ApiResponseFactory.Success("API Key rotated successfully.", new { client.ApiKey }));
        }


    }
}

using EStoreX.Core.DTO;
using EStoreX.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_StoreX.API.Controllers.Admin
{
    /// <summary>
    /// api clients controller  
    /// </summary>
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
                return BadRequest("Client name is required.");

            var client = await _clientService.CreateClientAsync(request.ClientName);

            return Ok(new RegisterApiClientResponse
            {
                ClientName = client.ClientName,
                ApiKey = client.ApiKey
            });
        }
    }
}

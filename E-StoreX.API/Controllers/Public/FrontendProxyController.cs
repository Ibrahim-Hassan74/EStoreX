using Microsoft.AspNetCore.Mvc;
using EStoreX.Core.DTO.Account.Requests;

namespace E_StoreX.API.Controllers.Public
{
    /// <summary>
    /// Controller that acts as a proxy between the frontend and backend for password reset functionality.
    /// </summary>
    /// <remarks>
    /// This controller is useful when the frontend cannot directly call the backend API due to security restrictions,
    /// such as the need to hide the API key.
    /// </remarks>
    [Route("api/frontend/reset")]
    [ApiController]
    public class FrontendProxyController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Initializes a new instance of the <see cref="FrontendProxyController"/> class.
        /// </summary>
        /// <param name="httpClientFactory">The factory used to create <see cref="HttpClient"/> instances.</param>
        /// <param name="configuration">The configuration provider used to access API settings.</param>
        /// <param name="httpContextAccessor">
        /// Provides access to the current HTTP context, allowing retrieval of request-specific data
        /// such as the scheme, host, and path for constructing full URLs.
        /// </param>

        public FrontendProxyController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// Acts as a proxy endpoint that forwards a password reset request from the frontend to the backend API.
        /// </summary>
        /// <param name="request">The password reset request body sent from the frontend.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> that contains the response returned by the backend API.
        /// Possible status codes include:
        /// - 200 OK if the reset was successful,
        /// - 4xx for client-side errors (e.g. invalid input, rate limits),
        /// - 5xx for server errors or connectivity issues.
        /// </returns>
        /// <remarks>
        /// This method reads the backend API URL and API key from the configuration, builds the full URL using the
        /// current HTTP request context, and forwards the request to the backend with the necessary headers.
        /// </remarks>

        [HttpPost]
        public async Task<IActionResult> ResetPasswordProxy([FromBody] ResetPasswordDTO request)
        {
            var apiKey = _configuration["ApiSettings:XApiKey"];
            var apiPath = _configuration["ApiSettings:BackendResetPasswordUrl"];

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to determine host information.");

            var requestUrl = httpContext.Request;
            var baseUrl = $"{requestUrl.Scheme}://{requestUrl.Host.Value}";

            var fullApiUrl = $"{baseUrl.TrimEnd('/')}/{apiPath}";

            using (var client = _httpClientFactory.CreateClient())
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Post, fullApiUrl)
                {
                    Content = JsonContent.Create(request)
                };
                httpRequest.Headers.Add("X-API-KEY", apiKey);

                var response = await client.SendAsync(httpRequest);
                var content = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, content);
            }
        }
        /// <summary>
        /// Acts as a proxy endpoint that forwards a verify reset password request from the frontend to the backend API.
        /// </summary>
        /// <param name="request">
        /// A <see cref="VerifyResetPasswordDTO"/> that contains the user's ID and the reset token.
        /// These parameters are passed from the frontend via query string.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> that contains the response from the backend.
        /// Status codes returned:
        /// - 200 OK: if the token is valid,
        /// - 400 Bad Request: if the token is invalid or expired,
        /// - 500 Internal Server Error: if the HTTP context is unavailable or another error occurs.
        /// </returns>
        /// <remarks>
        /// This endpoint constructs the backend verification URL using the current request scheme and host, 
        /// appends the API key in the headers, and relays the GET request with query parameters.
        /// </remarks>
        [HttpGet("verify")]
        public async Task<IActionResult> VerifyResetPassword([FromQuery] VerifyResetPasswordDTO request)
        {
            var apiKey = _configuration["ApiSettings:XApiKey"];
            var apiPath = _configuration["ApiSettings:BackendVerifyResetPasswordUrl"];

            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
                return StatusCode(StatusCodes.Status500InternalServerError, "Unable to determine host information.");

            var baseUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
            var fullBaseUrl = $"{baseUrl}/{apiPath}".TrimEnd('/');

            var urlWithParams = $"{fullBaseUrl}?userId={Uri.EscapeDataString(request.UserId)}&token={Uri.EscapeDataString(request.Token)}";

            using (var client = _httpClientFactory.CreateClient())
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, urlWithParams);
                httpRequest.Headers.Add("X-API-KEY", apiKey);

                var response = await client.SendAsync(httpRequest);
                var content = await response.Content.ReadAsStringAsync();

                return StatusCode((int)response.StatusCode, content);
            }
        }
    }
}

using EStoreX.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling user authentication-related actions
    /// such as registration and login for the E-StoreX API.
    /// </summary>
    public class AccountController : CustomControllerBase
    {
        private readonly IAuthenticationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="authService">The authentication service to handle login and registration logic.</param>
        public AccountController(IAuthenticationService authService)
        {
            _authService = authService;
        }
        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerDTO">User registration data</param>
        /// <returns>Result of registration</returns>
        [HttpPost("register")]
        public async Task<IActionResult> PostRegister([FromBody] RegisterDTO registerDTO)
        {
            var response = await _authService.RegisterAsync(registerDTO);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Login an existing user
        /// </summary>
        /// <param name="loginDTO">User login data</param>
        /// <returns>JWT token and login result</returns>
        [HttpPost("login")]
        public async Task<IActionResult> PostLogin([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.LoginAsync(loginDTO);
            return StatusCode(response.StatusCode, response);
        }
        /// <summary>
        /// Confirms a user's email using the provided user ID and token.
        /// </summary>
        /// <param name="dto">Contains the user ID, confirmation token, and optional redirect URL.</param>
        /// <returns>Returns a redirection if a redirect URL is provided; otherwise, a response object.</returns>
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.UserId) || string.IsNullOrEmpty(dto.Token))
            {
                return BadRequest("Invalid confirmation data.");
            }
            var response = await _authService.ConfirmEmailAsync(dto);
            //if (!string.IsNullOrEmpty(dto.RedirectTo))
            //{
            //    return Redirect(dto.RedirectTo);
            //}
            return StatusCode(response.StatusCode, response);
        }

    }
}

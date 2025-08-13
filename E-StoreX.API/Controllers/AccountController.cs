using AutoMapper;
using Domain.Entities;
using E_StoreX.API.Helper;
using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using System.Security.Claims;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling user authentication-related actions
    /// such as registration, login, email confirmation, and password reset for the E-StoreX API.
    /// </summary>
    public class AccountController : CustomControllerBase
    {
        private readonly IAuthenticationService _authService;
        private readonly IMapper _mapper;
        private readonly SignInManager<ApplicationUser> _signInManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="authService">
        /// The authentication service that handles user registration, login, 
        /// email confirmation, and password reset logic.
        /// </param>
        /// <param name="mapper">
        /// mapper instance for mapping between DTOs and domain entities.
        /// </param>
        /// <param name="signInManager">
        /// sign-in manager for handling user sign-in operations,
        /// </param>
        public AccountController(IAuthenticationService authService, IMapper mapper, SignInManager<ApplicationUser> signInManager)
        {
            _authService = authService;
            _mapper = mapper;
            _signInManager = signInManager;
        }

        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="registerDTO">
        /// An object containing user registration details like username, email, and password.
        /// </param>
        /// <returns>
        /// Returns <c>200 OK</c> if registration is successful, or <c>400/409</c> with details if it fails.
        /// </returns>
        [HttpPost("register")]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> PostRegister([FromBody] RegisterDTO registerDTO)
        {
            var response = await _authService.RegisterAsync(registerDTO);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Authenticates an existing user and generates a JWT token.
        /// </summary>
        /// <param name="loginDTO">
        /// The user's login credentials (email and password).
        /// </param>
        /// <returns>
        /// Returns a JWT token with <c>200 OK</c> on success or <c>401/404</c> with error details if authentication fails.
        /// </returns>
        [HttpPost("login")]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> PostLogin([FromBody] LoginDTO loginDTO)
        {
            var response = await _authService.LoginAsync(loginDTO);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Confirms a user's email using the provided user ID and token.
        /// </summary>
        /// <param name="dto">
        /// Contains the user ID, email confirmation token, and an optional redirect URL.
        /// </param>
        /// <returns>
        /// Returns <c>200 OK</c> if the email confirmation is successful, or 
        /// <c>400/404</c> if the token is invalid or user is not found.
        /// </returns>
        [HttpGet("confirm-email")]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> ConfirmEmail([FromQuery] ConfirmEmailDTO dto)
        {
            if (dto == null || string.IsNullOrEmpty(dto.UserId) || string.IsNullOrEmpty(dto.Token))
            {
                return BadRequest(ApiResponseFactory.BadRequest("Invalid confirmation data."));// BadRequest();
            }
            var response = await _authService.ConfirmEmailAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Sends a password reset link to the user's email address.
        /// </summary>
        /// <param name="dto">
        /// Contains the email address of the user who requested a password reset.
        /// </param>
        /// <returns>
        /// Returns <c>200 OK</c> if the reset link was sent successfully, or <c>400/429</c> with error details.
        /// </returns>
        [HttpPost("forgot-password")]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDTO dto)
        {
            var response = await _authService.ForgotPasswordAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Verifies the validity of a reset password token for a given user.
        /// </summary>
        /// <param name="dto">
        /// Contains the User ID and reset token to validate.
        /// </param>
        /// <returns>
        /// Returns <c>200 OK</c> if the token is valid or <c>400/404</c> if invalid/expired.
        /// </returns>
        [HttpGet("reset-password/verify")]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> VerifyResetPassword([FromQuery] VerifyResetPasswordDTO dto)
        {
            var response = await _authService.VerifyResetPasswordTokenAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Resets the user's password using a valid token.
        /// </summary>
        /// <param name="dto">Contains user ID, token, and new password details.</param>
        /// <returns>Returns a success or failure response based on token validity and password rules.</returns>
        [HttpPost("reset-password")]
        [Authorize("NotAuthorized")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var result = await _authService.ResetPasswordAsync(dto);
            return StatusCode(result.StatusCode, result);
        }
        /// <summary>
        /// Generates a new access token (and refresh token) using a valid refresh token.
        /// </summary>
        /// <param name="model">The current (expired) access token and refresh token.</param>
        /// <returns>Returns a new JWT token pair on success or an error response on failure.</returns>
        [HttpPost("generate-new-jwt-token")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel model)
        {
            var response = await _authService.RefreshTokenAsync(model);
            return StatusCode(response.StatusCode, response);
        }

        /// <summary>
        /// Updates the authenticated user's address.
        /// </summary>
        /// <param name="addressDTO">
        /// The new <see cref="ShippingAddressDTO"/> object containing the updated address details.
        /// </param>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with a success message if the update succeeds,
        /// or <see cref="BadRequestObjectResult"/> with an error message if it fails.
        /// </returns>
        /// <remarks>
        /// The method retrieves the user's email from the JWT claims and uses it to update the address.
        /// The user must be authenticated for this operation.
        /// </remarks>
        [Authorize]
        [HttpPut("update-address")]
        public async Task<IActionResult> UpdateAddress([FromBody] ShippingAddressDTO addressDTO)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var address = _mapper.Map<Address>(addressDTO);
            bool ok = await _authService.UpdateAddress(email, address);
            if (ok)
                return Ok(new { message = "Address updated successfully." });
            return BadRequest(ApiResponseFactory.BadRequest("Failed to update address."));
        }

        /// <summary>
        /// Retrieves the shipping address of the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// Requires the user to be authenticated. The user's email is extracted from the JWT claims,
        /// and used to fetch the associated shipping address from the authentication service.
        /// </remarks>
        /// <returns>
        /// Returns <see cref="OkObjectResult"/> with the shipping address on success (HTTP 200),
        /// or <see cref="BadRequestObjectResult"/> with an error message if the address could not be retrieved (HTTP 400).
        /// </returns>
        [HttpGet("get-address")]
        [Authorize]
        public async Task<IActionResult> GetAddress()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var shippingAddress = await _authService.GetAddress(email);
            if (shippingAddress == null)
                return BadRequest(ApiResponseFactory.BadRequest("Failed to get address."));
            return Ok(shippingAddress);
        }
        /// <summary>
        /// Logs out the currently authenticated user by clearing their refresh token 
        /// and signing them out.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the logout response status and message.
        /// </returns>
        /// <remarks>
        /// Requires the user to be authenticated.
        /// </remarks>
        [HttpGet("logout")]
        [Authorize]
        public async Task<IActionResult> PostLogout()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var response = await _authService.LogoutAsync(email);
            return StatusCode(response.StatusCode, response);
        }
        /// <summary>
        /// Retrieves information about the currently authenticated user.
        /// </summary>
        /// <remarks>
        /// This endpoint reads the user ID from the authentication token claims
        /// and returns the corresponding user details.
        /// </remarks>
        /// <returns>
        /// 200 OK with <see cref="ApplicationUserResponse"/> if the user exists;
        /// 400 Bad Request if no user ID is found in claims;
        /// 404 Not Found if the user does not exist.
        /// </returns>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return BadRequest(ApiResponseFactory.BadRequest("User ID not found in claims."));

            var userResponse = await _authService.GetUserByIdAsync(userId);
            if (userResponse == null)
                return NotFound(ApiResponseFactory.NotFound("User not found."));

            return Ok(userResponse);
        }
        /// <summary>
        /// Updates the authenticated user's profile information, including display name, 
        /// phone number, and optionally their password.
        /// </summary>
        /// <param name="dto">
        /// An <see cref="UpdateUserDTO"/> object containing the new profile data, 
        /// including current password and new password if the password is being changed.
        /// </param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the result of the update operation.
        /// On success, returns <see cref="AuthenticationResponse"/> with status code 200.
        /// On failure, returns <see cref="AuthenticationFailureResponse"/> with appropriate error details.
        /// </returns>

        [HttpPut("update-profile")]
        [Authorize]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserDTO dto)
        {
            var userIdFromToken = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdFromToken) || userIdFromToken != dto.UserId)
                return StatusCode(StatusCodes.Status403Forbidden, ApiResponseFactory.Forbidden());

            var result = await _authService.UpdateUserProfileAsync(dto);
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Initiates the external login process by redirecting the user to the chosen authentication provider (e.g., Google, GitHub).
        /// </summary>
        /// <param name="provider">
        /// The external authentication provider to use (e.g., "Google", "GitHub").
        /// </param>
        /// <returns>
        /// A challenge result that redirects the user to the external provider's login page.
        /// </returns>
        /// <remarks>
        /// This method configures the external authentication properties, sets the redirect URL 
        /// to handle the provider's callback, and prompts the user to select an account.
        /// </remarks>
        [HttpGet("external-login")]
        public IActionResult ExternalLogin([FromQuery] string provider)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), nameof(AccountController)) ?? "api/Account/external-login-callback";

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            properties.Items["prompt"] = "select_account";

            return Challenge(properties, provider);
        }

        /// <summary>
        /// Handles the callback from the external login provider after the authentication attempt.
        /// </summary>
        /// <param name="remoteError">
        /// Optional error message returned by the external provider during the authentication process.
        /// </param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the authentication response, 
        /// including status code and any relevant data or error messages.
        /// </returns>
        /// <remarks>
        /// This method calls <see cref="_authService.ExternalLoginCallbackAsync"/> to process the 
        /// external login result and return the final authentication response.
        /// </remarks>
        [HttpGet("external-login-callback")]
        public async Task<IActionResult> ExternalLoginCallback(string remoteError = "")
        {
            var response = await _authService.ExternalLoginCallbackAsync(remoteError);
            return StatusCode(response.StatusCode, response);
        }


    }
}

using AutoMapper;
using Domain.Entities;
using EStoreX.Core.DTO;
using Microsoft.AspNetCore.Authorization;
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
        public AccountController(IAuthenticationService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
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
                return BadRequest("Invalid confirmation data.");
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
            return BadRequest(new { message = "Failed to update address." });
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
                return BadRequest(new { message = "Failed to get address." });
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


    }
}

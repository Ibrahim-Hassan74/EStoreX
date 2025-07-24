using EStoreX.API.Filters;
using EStoreX.Core.DTO;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace E_StoreX.API.Controllers
{
    /// <summary>
    /// Controller responsible for handling user authentication-related actions
    /// such as registration, login, email confirmation, and password reset for the E-StoreX API.
    /// </summary>
    public class AccountController : CustomControllerBase
    {
        private readonly IAuthenticationService _authService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="authService">
        /// The authentication service that handles user registration, login, 
        /// email confirmation, and password reset logic.
        /// </param>
        public AccountController(IAuthenticationService authService)
        {
            _authService = authService;
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
        public async Task<IActionResult> VerifyResetPassword([FromQuery] VerifyResetPasswordDTO dto)
        {
            var response = await _authService.VerifyResetPasswordTokenAsync(dto);
            return StatusCode(response.StatusCode, response);
        }

        ///// <summary>
        ///// Resets the user's password using a valid token.
        ///// </summary>
        ///// <param name="dto">Contains user ID, token, and new password details.</param>
        ///// <returns>Returns a success or failure response based on token validity and password rules.</returns>
        //[HttpPost("reset-password")]
        //public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        //{
        //    var result = await _authService.ResetPasswordAsync(dto);
        //    return StatusCode(result.StatusCode, result);
        //}
    }
}

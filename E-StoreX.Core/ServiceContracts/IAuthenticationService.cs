using EStoreX.Core.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Provides authentication-related operations such as user registration, login, email confirmation, and password reset.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Registers a new user with the provided registration data.
        /// </summary>
        /// <param name="registerDTO">Data required for registering a new user.</param>
        /// <returns>Returns a response indicating success or failure of registration.</returns>
        Task<AuthenticationResponse> RegisterAsync(RegisterDTO registerDTO);

        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// </summary>
        /// <param name="loginDTO">Login data including email and password.</param>
        /// <returns>Returns a response indicating success or failure of login.</returns>
        Task<AuthenticationResponse> LoginAsync(LoginDTO loginDTO);

        /// <summary>
        /// Confirms a user's email address using the confirmation token.
        /// </summary>
        /// <param name="dto">Contains user ID and email confirmation token.</param>
        /// <returns>Returns a response indicating whether the email confirmation was successful.</returns>
        Task<AuthenticationResponse> ConfirmEmailAsync(ConfirmEmailDTO dto);

        /// <summary>
        /// Initiates the forgot password process by sending a reset link to the user's email.
        /// </summary>
        /// <param name="dto">Contains the email address of the user requesting password reset.</param>
        /// <returns>Returns a response indicating whether the reset email was sent successfully.</returns>
        Task<AuthenticationResponse> ForgotPasswordAsync(ForgotPasswordDTO dto);
    }
}

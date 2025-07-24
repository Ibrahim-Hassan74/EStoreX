using EStoreX.Core.DTO;

namespace ServiceContracts
{
    /// <summary>
    /// Provides authentication-related operations such as user registration, login, 
    /// email confirmation, and password reset.
    /// </summary>
    public interface IAuthenticationService
    {
        /// <summary>
        /// Registers a new user with the provided registration data.
        /// </summary>
        /// <param name="registerDTO">
        /// Data required for registering a new user.
        /// </param>
        /// <returns>
        /// Returns an <see cref="AuthenticationResponse"/> indicating whether the 
        /// registration was successful (StatusCode 200) or failed with details 
        /// (e.g., StatusCode 400 or 409 if username/email is already in use).
        /// </returns>
        Task<AuthenticationResponse> RegisterAsync(RegisterDTO registerDTO);

        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// </summary>
        /// <param name="loginDTO">
        /// Login data including email and password.
        /// </param>
        /// <returns>
        /// Returns an <see cref="AuthenticationResponse"/> indicating whether the 
        /// login was successful (StatusCode 200) or failed due to invalid credentials, 
        /// unconfirmed email, or locked account.
        /// </returns>
        Task<AuthenticationResponse> LoginAsync(LoginDTO loginDTO);

        /// <summary>
        /// Confirms a user's email address using the confirmation token.
        /// </summary>
        /// <param name="dto">
        /// Contains the user ID and email confirmation token.
        /// </param>
        /// <returns>
        /// Returns an <see cref="AuthenticationResponse"/> indicating whether the 
        /// email confirmation was successful (StatusCode 200) or failed due to an 
        /// invalid/expired token (StatusCode 400 or 404).
        /// </returns>
        Task<AuthenticationResponse> ConfirmEmailAsync(ConfirmEmailDTO dto);

        /// <summary>
        /// Initiates the forgot password process by generating a reset token and 
        /// sending a reset link to the user's email.
        /// </summary>
        /// <param name="dto">
        /// Contains the email address of the user requesting password reset.
        /// </param>
        /// <returns>
        /// Returns an <see cref="AuthenticationResponse"/> indicating whether the 
        /// reset email was sent successfully (StatusCode 200) or failed due to 
        /// invalid email or unconfirmed account.
        /// </returns>
        Task<AuthenticationResponse> ForgotPasswordAsync(ForgotPasswordDTO dto);

        /// <summary>
        /// Verifies the validity of a reset password token for a specific user.
        /// </summary>
        /// <param name="dto">
        /// A <see cref="VerifyResetPasswordDTO"/> containing the User ID and the 
        /// reset password token that needs to be verified.
        /// </param>
        /// <returns>
        /// Returns an <see cref="AuthenticationResponse"/> indicating whether the 
        /// token is valid (StatusCode 200) or invalid/expired (StatusCode 400 or 404).
        /// </returns>
        Task<AuthenticationResponse> VerifyResetPasswordTokenAsync(VerifyResetPasswordDTO dto);

        /// <summary>
        /// Resets the user's password using a valid reset token.
        /// </summary>
        /// <param name="dto">
        /// Contains user ID, reset token, new password, and confirmation password.
        /// </param>
        /// <returns>
        /// Returns an <see cref="AuthenticationResponse"/> indicating whether the 
        /// password reset was successful (StatusCode 200) or failed due to token 
        /// issues or validation errors (StatusCode 400 or 404).
        /// </returns>
        Task<AuthenticationResponse> ResetPasswordAsync(ResetPasswordDTO dto);
        /// <summary>
        /// Generates a new JWT (and refresh token) using a valid refresh token.
        /// </summary>
        /// <param name="model">The current (expired) access token and the refresh token.</param>
        /// <returns>
        /// Returns an <see cref="AuthenticationResponse"/> with a new access token on success,
        /// or a failure response when the refresh token is invalid or expired.
        /// </returns>
        Task<AuthenticationResponse> RefreshTokenAsync(TokenModel model);
    }
}

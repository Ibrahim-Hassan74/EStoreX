namespace EStoreX.Core.DTO
{
    /// <summary>
    /// DTO used to verify the reset password token before allowing the user to set a new password.
    /// </summary>
    public class VerifyResetPasswordDTO
    {
        /// <summary>
        /// Gets or sets the User ID of the account requesting password reset.
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the reset password token sent via email.
        /// </summary>
        public string Token { get; set; } = string.Empty;
    }
}

using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Account.Requests
{
    /// <summary>
    /// Data Transfer Object for confirming a user's email.
    /// </summary>
    public class ConfirmEmailDTO
    {
        /// <summary>
        /// The unique identifier of the user to confirm the email for.
        /// </summary>
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// The token used to confirm the user's email address.
        /// </summary>
        [Required(ErrorMessage = "Confirmation token is required.")]
        [MinLength(10, ErrorMessage = "The confirmation token must be at least 10 characters long.")]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Optional URL to redirect the user to after confirmation.
        /// </summary>
        [Url(ErrorMessage = "RedirectTo must be a valid URL.")]
        public string? RedirectTo { get; set; }
    }
}

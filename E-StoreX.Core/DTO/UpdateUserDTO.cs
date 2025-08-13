using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO
{
    public class UpdateUserDTO : IValidatableObject
    {
        [Required(ErrorMessage = "User ID is required.")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Display Name is required.")]
        public string? DisplayName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number.")]
        public string? PhoneNumber { get; set; } = string.Empty;

        public string? CurrentPassword { get; set; } = string.Empty;

        public string? NewPassword { get; set; } = string.Empty;

        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmNewPassword { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(NewPassword))
            {
                if (string.IsNullOrEmpty(CurrentPassword))
                {
                    yield return new ValidationResult(
                        "Current password is required to set a new password.",
                        new[] { nameof(CurrentPassword) });
                }

                if (string.IsNullOrEmpty(ConfirmNewPassword))
                {
                    yield return new ValidationResult(
                        "Please confirm the new password.",
                        new[] { nameof(ConfirmNewPassword) });
                }
            }
        }
    }
}

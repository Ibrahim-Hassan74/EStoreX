using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Account.Requests
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        [Display(Name = "User ID")]
        public string? UserId { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        public string? Token { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "{0} can't be blank")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string? ConfirmPassword { get; set; }
    }
}

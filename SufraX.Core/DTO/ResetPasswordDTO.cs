using System;
using System.ComponentModel.DataAnnotations;

namespace SufraX.Core.DTO
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        [EmailAddress(ErrorMessage = "{0} Should be in proper email address format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

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

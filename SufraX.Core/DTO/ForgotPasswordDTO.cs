using System.ComponentModel.DataAnnotations;

namespace SufraX.Core.DTO
{
    public class ForgotPasswordDTO
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        [EmailAddress(ErrorMessage = "{0} Should be in proper email address format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
    }
}

using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Account.Requests
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "{0} can't be blank")]
        [EmailAddress(ErrorMessage = "{0} Should be in proper email address format")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required(ErrorMessage = "{0} can't be blank")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "{0} should be at least 7 characters")]
        public string? Password { get; set; }
        public bool RememberMe { get; set; } = false;
        //public IEnumerable<AuthenticationScheme> Schemes { get; set; } = new List<AuthenticationScheme>();
    }
}

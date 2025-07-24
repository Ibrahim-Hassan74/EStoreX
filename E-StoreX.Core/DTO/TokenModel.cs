using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO
{
    public class TokenModel
    {
        [Required(ErrorMessage = "{0} is required.")]
        public string? Token { get; set; } = string.Empty;
        [Required(ErrorMessage = "{0} is required.")]
        public string? RefreshToken { get; set; } = string.Empty;
    }
}

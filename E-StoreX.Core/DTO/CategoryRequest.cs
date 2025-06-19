using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO
{
    public record CategoryRequest([Required(ErrorMessage = "{0} can't be blank")] string Name,
        [Required][StringLength(1000, ErrorMessage = "{0} must be between {1} and {2}", MinimumLength = 5)]string Description);
}

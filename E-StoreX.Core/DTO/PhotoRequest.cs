using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO
{
    public record PhotoRequest(
        [Required]
    [MaxLength(200)]
    string ImageName);

}
using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Products.Requests
{
    public record PhotoRequest(
        [Required]
    [MaxLength(200)]
    string ImageName);

}
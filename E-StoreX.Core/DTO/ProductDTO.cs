using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO
{
    public record ProductRequest
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string? Description { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Price must be non-negative.")]
        public decimal? Price { get; set; }

        [Required]
        public string CategoryName { get; set; } = string.Empty;

        [MinLength(1, ErrorMessage = "At least one photo is required.")]
        public List<PhotoRequest> Photos { get; set; } = new();
    }

    public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal? Price,
    string CategoryName,
    List<PhotoResponse> Photos);
    public record PhotoRequest(
        [Required]
    [MaxLength(200)]
    string Name);
    public record PhotoResponse(string Name, Guid ProductId);

}

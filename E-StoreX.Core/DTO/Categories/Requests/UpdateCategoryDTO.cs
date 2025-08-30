using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Categories.Requests
{
    //public record UpdateCategoryDTO([Required(ErrorMessage = "{0} can't be blank")]string Name,
    //    [Required][StringLength(1000, ErrorMessage = "{0} must be between {1} and {2}", MinimumLength = 5)] string Description, 
    //    [Required(ErrorMessage = "{0} can't be blank")]Guid Id);
    public class UpdateCategoryDTO
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "{0} can't be blank")]
        public string Name { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [StringLength(1000, ErrorMessage = "{0} must be between {2} and {1}", MinimumLength = 5)]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "{0} can't be blank")]
        public Guid Id { get; set; }
    }
}

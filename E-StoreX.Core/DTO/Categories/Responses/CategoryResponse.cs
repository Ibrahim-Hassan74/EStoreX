using EStoreX.Core.DTO.Products.Responses;

namespace EStoreX.Core.DTO.Categories.Responses
{
    public record CategoryResponse(Guid Id, string Name, string Description);
    public class CategoryResponseWithPhotos
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<PhotoResponse> Photos { get; set; }
    }
}

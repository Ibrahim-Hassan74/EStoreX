using EStoreX.Core.DTO.Products.Responses;

namespace EStoreX.Core.DTO.Brands.Response
{
    public class BrandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<PhotoResponse> Photos { get; set; } = new List<PhotoResponse>();
    }
}

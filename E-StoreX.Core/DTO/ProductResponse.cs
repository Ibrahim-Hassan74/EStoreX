namespace EStoreX.Core.DTO
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string CategoryName { get; set; }
        public IEnumerable<PhotoResponse> Photos { get; set; }

        public ProductResponse() { } 
    }
}

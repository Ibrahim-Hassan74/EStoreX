namespace EStoreX.Core.DTO.Products.Responses
{
    public class ProductResponse
    {
        public Guid Id { get; set; }
        // NameEn and DescriptionEn are localized values chosen at mapping time
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public string CategoryName { get; set; }
        public int QuantityAvailable { get; set; }
        public string BrandName { get; set; }
        public bool IsFeatured { get; set; }
        public IEnumerable<PhotoResponse> Photos { get; set; }

        public ProductResponse() { } 
    }
    public class ProductResponseWithDetails
    {
        public Guid Id { get; set; }
        // NameEn and DescriptionEn are localized values chosen at mapping time
        public string NameAr { get; set; }
        public string DescriptionAr { get; set; }
        public string NameEn { get; set; }
        public string DescriptionEn { get; set; }
        public decimal NewPrice { get; set; }
        public decimal OldPrice { get; set; }
        public string CategoryName { get; set; }
        public int QuantityAvailable { get; set; }
        public string BrandName { get; set; }
        public Guid CategoryId { get; set; }
        public Guid BrandId { get; set; }
        public bool IsFeatured { get; set; }
        public IEnumerable<ProductImageResponse> Photos { get; set; }

        public ProductResponseWithDetails() { }
    }
}

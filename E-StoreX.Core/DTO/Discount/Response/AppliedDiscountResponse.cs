namespace EStoreX.Core.DTO.Discount.Response
{
    public class AppliedDiscountResponse
    {
        public string Product { get; set; } = string.Empty;
        public decimal OriginalPrice { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
    }
}

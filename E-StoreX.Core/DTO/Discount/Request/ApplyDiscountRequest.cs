namespace EStoreX.Core.DTO.Discount.Request
{
    /// <summary>
    /// Request model for applying a discount to a product.
    /// </summary>
    public class ApplyDiscountRequest
    {
        /// <summary>
        /// The unique identifier of the product to which the discount should be applied.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// The discount code to apply.
        /// </summary>
        public string Code { get; set; } = string.Empty;
    }
}

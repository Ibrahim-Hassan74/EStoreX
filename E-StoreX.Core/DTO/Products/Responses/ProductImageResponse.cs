namespace EStoreX.Core.DTO.Products.Responses
{
    public class ProductImageResponse
    {
        /// <summary>
        /// Unique identifier of the image.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The full URL or relative path of the image.
        /// </summary>
        public string Url { get; set; } = string.Empty;
    }
}

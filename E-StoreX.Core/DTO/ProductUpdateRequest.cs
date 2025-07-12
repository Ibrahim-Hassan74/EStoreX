using System;
using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO
{
    public class ProductUpdateRequest : ProductAddRequest
    {
        /// <summary>
        /// Gets or sets the unique identifier for the product.
        /// </summary>
        [Required(ErrorMessage = "{0} can't be blank")]
        public Guid Id { get; set; }
    }
}

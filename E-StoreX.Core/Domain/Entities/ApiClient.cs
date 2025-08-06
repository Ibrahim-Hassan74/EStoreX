using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.Domain.Entities
{
    public class ApiClient : BaseEntity<Guid>
    {
        public string? ClientName { get; set; }
        [Required(ErrorMessage = "API Key is required.")]
        public string ApiKey { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

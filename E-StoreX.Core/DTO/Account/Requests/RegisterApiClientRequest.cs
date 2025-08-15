using System.ComponentModel.DataAnnotations;

namespace EStoreX.Core.DTO.Account.Requests
{
    public class RegisterApiClientRequest
    {
        [Required(ErrorMessage = "API Key is required.")]
        public string ClientName { get; set; } = null!;
    }
}

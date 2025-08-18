namespace EStoreX.Core.DTO.Account.Responses
{
    public class RegisterApiClientResponse
    {
        public Guid ClientId { get; set; }
        public string ClientName { get; set; } = null!;
        public string ApiKey { get; set; } = null!;
    }
}

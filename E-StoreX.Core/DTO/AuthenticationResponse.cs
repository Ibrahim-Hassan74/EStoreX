namespace EStoreX.Core.DTO
{
    public class AuthenticationResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }
    }

}

namespace EStoreX.Core.DTO
{
    public class AuthenticationFailureResponse : AuthenticationResponse
    {
        public List<string> Errors { get; set; } = new List<string>();  
    }
}

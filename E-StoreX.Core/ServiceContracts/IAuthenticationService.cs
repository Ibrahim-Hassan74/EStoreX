using EStoreX.Core.DTO;

namespace ServiceContracts
{
    public interface IAuthenticationService
    {
        Task<AuthenticationResponse> RegisterAsync(RegisterDTO registerDTO);
        Task<AuthenticationResponse> LoginAsync(LoginDTO loginDTO);
    }
}

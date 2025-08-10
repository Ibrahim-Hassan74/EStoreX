using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO;
using System.Security.Claims;

namespace EStoreX.Core.ServiceContracts
{
    public interface IJwtService
    {
        Task<AuthenticationResponse> CreateJwtToken(ApplicationUser user);
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}

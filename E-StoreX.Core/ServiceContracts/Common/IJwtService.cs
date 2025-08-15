using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO.Common;
using System.Security.Claims;

namespace EStoreX.Core.ServiceContracts.Common
{
    /// <summary>
    /// JWT Service Interface
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JWT token using the given user's information and the configuration settings.
        /// </summary>
        /// <param name="user">ApplicationUser object</param>
        /// <returns>AuthenticationResponse that includes token</returns>
        Task<ApiResponse> CreateJwtToken(ApplicationUser user);
        /// <summary>
        /// Get ClaimsPrincipal from JWT token
        /// </summary>
        /// <param name="token">Access Token</param>
        /// <returns></returns>
        ClaimsPrincipal? GetPrincipalFromJwtToken(string? token);
    }
}

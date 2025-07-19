using EStoreX.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity;
using ServiceContracts;

namespace EStoreX.Infrastructure.Repository
{
    public class Authentication : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public Authentication(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityResult> RegisterAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

    }
}

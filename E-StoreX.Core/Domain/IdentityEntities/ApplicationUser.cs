using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace EStoreX.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? DisplayName { get; set; }
        public Address Address { get; set; }
        public string? LastEmailConfirmationToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDateTime { get; set; }
    }
}

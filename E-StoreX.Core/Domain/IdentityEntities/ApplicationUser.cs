using Domain.Entities.Common;
using EStoreX.Core.Domain.Entities.Rating;
using Microsoft.AspNetCore.Identity;

namespace EStoreX.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? DisplayName { get; set; }
        public Address Address { get; set; }
        public string? LastEmailConfirmationToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpirationDateTime { get; set; }
        public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    }
}

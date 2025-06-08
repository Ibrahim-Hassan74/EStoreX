using Microsoft.AspNetCore.Identity;
using EStoreX.Core.Domain.Entities;

namespace EStoreX.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
        public string? LastEmailConfirmationToken { get; set; }
        public DateTime? LastResendTime { get; set; }
    }
}

using Microsoft.AspNetCore.Identity;
using SufraX.Core.Domain.Entities;

namespace SufraX.Core.Domain.IdentityEntities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string? PersonName { get; set; }
        public ICollection<Order>? Orders { get; set; }
        public string? LastEmailConfirmationToken { get; set; }
        public DateTime? LastResendTime { get; set; }
    }
}

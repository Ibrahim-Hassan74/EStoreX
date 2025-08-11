using AutoMapper;
using EStoreX.Core.Domain.IdentityEntities;
using EStoreX.Core.DTO;

namespace EStoreX.Core.Mapping
{
    public class ApplicationUserMapping : Profile
    {
        public ApplicationUserMapping()
        {
            CreateMap<ApplicationUser, ApplicationUserResponse>()
            .ForMember(dest => dest.Roles, opt => opt.Ignore())
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.LockoutEnd == null || src.LockoutEnd < DateTime.UtcNow));
        }
    }
}

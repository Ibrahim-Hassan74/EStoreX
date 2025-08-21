using AutoMapper;
using EStoreX.Core.Domain.Entities.Rating;
using EStoreX.Core.DTO.Ratings.Requests;
using EStoreX.Core.DTO.Ratings.Response;

namespace EStoreX.Core.Mapping
{
    public class RatingProfile : Profile
    {
        public RatingProfile()
        {
            CreateMap<RatingAddRequest, Rating>();
            CreateMap<RatingUpdateRequest, Rating>();
            CreateMap<Rating, RatingResponse>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.DisplayName ?? src.User.UserName));

        }
    }
}

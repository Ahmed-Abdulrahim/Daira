using Daira.Application.DTOs.LikeModule;

namespace Daira.Application.Mapping
{
    public class LikeMappingProfile : Profile
    {
        public LikeMappingProfile()
        {
            CreateMap<LikeDto, Like>().ReverseMap();
        }
    }
}

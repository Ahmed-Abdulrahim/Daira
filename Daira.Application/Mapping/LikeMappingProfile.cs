namespace Daira.Application.Mapping
{
    public class LikeMappingProfile : Profile
    {
        public LikeMappingProfile()
        {
            CreateMap<LikeResponse, Like>().ReverseMap();
        }
    }
}

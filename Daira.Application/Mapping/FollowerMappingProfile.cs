namespace Daira.Application.Mapping
{
    public class FollowerMappingProfile : Profile
    {
        public FollowerMappingProfile()
        {
            CreateMap<FollowerResponse, Follower>().ReverseMap();
        }
    }
}

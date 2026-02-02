namespace Daira.Application.Mapping
{
    public class FollowerMappingProfile : Profile
    {
        public FollowerMappingProfile()
        {
            CreateMap<FollowerDto, Follower>().ReverseMap();
        }
    }
}

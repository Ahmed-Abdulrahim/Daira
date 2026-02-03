namespace Daira.Application.Mapping
{
    public class FriendShipMappingProfile : Profile
    {
        public FriendShipMappingProfile()
        {
            CreateMap<FriendshipDto, Friendship>().ReverseMap();
        }
    }
}

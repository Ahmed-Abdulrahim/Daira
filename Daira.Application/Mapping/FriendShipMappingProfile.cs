namespace Daira.Application.Mapping
{
    public class FriendShipMappingProfile : Profile
    {
        public FriendShipMappingProfile()
        {
            CreateMap<FriendshipResponse, Friendship>().ReverseMap();
        }
    }
}

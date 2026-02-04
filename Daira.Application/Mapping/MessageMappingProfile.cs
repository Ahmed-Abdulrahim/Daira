namespace Daira.Application.Mapping
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<MessageResponse, Message>()
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender)).ReverseMap();

            CreateMap<AppUser, SenderDto>()
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.DisplayName))
               .ForMember(dest => dest.PictureUrl, opt => opt.MapFrom(src => src.PictureUrl));

        }
    }
}

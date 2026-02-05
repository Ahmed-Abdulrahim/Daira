namespace Daira.Application.Mapping
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<Notification, NotificationResponse>()
                .ForMember(dest => dest.Actor, opt => opt.MapFrom(src => src.Actor));


        }
    }
}

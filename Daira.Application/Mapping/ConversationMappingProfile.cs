using Daira.Application.Response.ConversationModule;

namespace Daira.Application.Mapping
{
    public class ConversationMappingProfile : Profile
    {
        public ConversationMappingProfile()
        {
            CreateMap<ParticipantDto, ConversationParticipant>().ReverseMap();
            CreateMap<MessageDto, Message>()
                .ForMember(dest => dest.Sender, opt => opt.MapFrom(src => src.Sender));
            CreateMap<ConversationResponse, Conversation>().ReverseMap();
        }
    }
}

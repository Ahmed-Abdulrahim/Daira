namespace Daira.Application.Response.ConversationModule
{
    public class ConversationResponse
    {
        public Guid Id { get; set; }
        public ConversationType Type { get; set; }
        public string Name { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<ParticipantDto> Participants { get; set; }
        public MessageDto? LastMessage { get; set; }
    }
}

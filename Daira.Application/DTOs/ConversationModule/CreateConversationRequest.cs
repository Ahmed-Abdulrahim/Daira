namespace Daira.Application.DTOs.ConversationModule
{
    public class CreateConversationRequest
    {
        public string Type { get; set; } = "Direct";
        public string Name { get; set; }
        public List<string> ParticipantIds { get; set; } = new();
    }
}

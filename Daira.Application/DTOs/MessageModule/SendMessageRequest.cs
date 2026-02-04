namespace Daira.Application.DTOs.MessageModule
{
    public class SendMessageRequest
    {
        public Guid ConversationId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}

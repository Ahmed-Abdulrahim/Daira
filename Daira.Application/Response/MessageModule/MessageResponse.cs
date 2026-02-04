namespace Daira.Application.Response.MessageModule
{
    public class MessageResponse
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public string SenderId { get; set; }
        public SenderDto? Sender { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

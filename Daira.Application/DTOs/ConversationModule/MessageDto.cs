namespace Daira.Application.DTOs.ConversationModule
{
    public class MessageDto
    {
        public Guid Id { get; set; }
        public Guid ConversationId { get; set; }
        public string SenderId { get; set; } = string.Empty;
        public SenderDto? Sender { get; set; }
        public string Content { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
    public class SenderDto
    {
        public string UserId { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? PictureUrl { get; set; }
    }
}

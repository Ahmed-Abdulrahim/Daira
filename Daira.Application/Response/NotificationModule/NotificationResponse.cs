namespace Daira.Application.Response.NotificationModule
{
    public class NotificationResponse
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? ActorId { get; set; }
        public SenderDto? Actor { get; set; }
        public NotificationType Type { get; set; }
        public Guid? TargetId { get; set; }
        public NotificationTarget? TargetType { get; set; }
        public string? Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

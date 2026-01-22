namespace Daira.Domain.Models
{
    public enum NotificationTarget
    {
        Post,
        Comment,
        User,
        Message
    }
    public enum NotificationType
    {
        Like,
        Comment,
        Follow,
        FriendRequest,
        Message,
    }
    public class Notification
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string? ActorId { get; set; }
        public NotificationType Type { get; set; }
        public Guid? TargetId { get; set; }
        public NotificationTarget? TargetType { get; set; }
        public string? Content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        //Navigation Properties
        public AppUser User { get; set; }
        public AppUser Actor { get; set; }
    }
}


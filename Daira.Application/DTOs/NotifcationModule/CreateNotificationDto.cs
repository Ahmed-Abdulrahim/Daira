namespace Daira.Application.DTOs.NotifcationModule
{
    public class CreateNotificationDto
    {
        public string UserId { get; set; }
        public string ActorId { get; set; }
        public NotificationType Type { get; set; }
        public Guid? targetId { get; set; } = Guid.Empty;
        public NotificationTarget targetType { get; set; }
        public string content { get; set; }
    }
}

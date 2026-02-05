namespace Daira.Infrastructure.Services.NotificationService
{
    public class NotificationService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<NotificationService> logger) : INotificationService
    {
        //Craete Notifcation
        public async Task<ResultResponse<NotificationResponse>> CreateNotification(CreateNotificationDto notificationDto, CancellationToken cancellationToken)
        {
            if (notificationDto.UserId == notificationDto.ActorId)
            {
                logger.LogDebug("Skipping notification creation for self-action by user {UserId}", notificationDto.UserId);
                return ResultResponse<NotificationResponse>.Success("Self-action notification skipped");
            }

            var notification = new Notification
            {
                UserId = notificationDto.UserId,
                ActorId = notificationDto.ActorId,
                Type = notificationDto.Type,
                TargetId = notificationDto.targetId,
                TargetType = notificationDto.targetType,
                Content = notificationDto.content ?? GenerateNotificationContent(notificationDto.Type),
                IsRead = false,
                CreatedAt = DateTime.UtcNow
            };

            await unitOfWork.Repository<Notification>().AddAsync(notification);
            await unitOfWork.CommitAsync();

            // Reload with includes for proper mapping
            var spec = new NotificiationSpecification(notification.Id);
            var savedNotification = await unitOfWork.Repository<Notification>()
                .GetByIdSpecTracked(spec, cancellationToken);

            var mapped = mapper.Map<NotificationResponse>(savedNotification);
            return ResultResponse<NotificationResponse>.Success(mapped, "Notification created successfully");
        }

        //Delete Notifcation
        public async Task<ResultResponse<NotificationResponse>> DeleteNotificationAsync(Guid notificationId, string userId, CancellationToken cancellationToken)
        {
            var spec = new NotificiationSpecification(n => n.Id == notificationId && n.UserId == userId);
            var notification = await unitOfWork.Repository<Notification>()
                .GetByIdSpecTracked(spec, cancellationToken);

            if (notification is null)
            {
                logger.LogWarning("Notification {NotificationId} not found for user {UserId}", notificationId, userId);
                return ResultResponse<NotificationResponse>.Failure("Notification not found");
            }

            unitOfWork.Repository<Notification>().Delete(notification);
            await unitOfWork.CommitAsync();

            logger.LogInformation("Notification {NotificationId} deleted", notificationId);
            return ResultResponse<NotificationResponse>.Success("Notification deleted successfully");
        }

        //Get Notification
        public async Task<ResultResponse<NotificationResponse>> GetNotificationUserAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var spec = new NotificiationSpecification(n => n.UserId == userId, pageNumber, pageSize);
            var getNotification = await unitOfWork.Repository<Notification>().GetAllWithSpec(spec, cancellationToken);
            if (!getNotification.Any())
            {
                logger.LogInformation("No notifications found for user {UserId}", userId);
                return ResultResponse<NotificationResponse>.Success("No notifications found");
            }
            var mapped = mapper.Map<List<NotificationResponse>>(getNotification);
            return ResultResponse<NotificationResponse>.Success(mapped);

        }

        //Get unReadNotification
        public async Task<ResultResponse<int>> GetUnreadCountAsync(string userId, CancellationToken cancellationToken)
        {
            var spec = new NotificiationSpecification(n => n.UserId == userId && !n.IsRead);
            var unreadNotifications = await unitOfWork.Repository<Notification>()
               .GetAllWithSpec(spec, cancellationToken);
            if (!unreadNotifications.Any())
            {
                logger.LogInformation("No  unread notifications");
                return ResultResponse<int>.Success("No unRead notifications");
            }

            var count = unreadNotifications.Count();
            return ResultResponse<int>.Success(count, $"User has {count} unread notifications");
        }

        //MarkAll As Read
        public async Task<ResultResponse<NotificationResponse>> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken)
        {
            var spec = new NotificiationSpecification(n => n.UserId == userId && !n.IsRead);
            var unreadNotifications = await unitOfWork.Repository<Notification>()
                .GetAllWithSpec(spec, cancellationToken);

            if (!unreadNotifications.Any())
            {
                return ResultResponse<NotificationResponse>.Success("No unread notifications to mark");
            }

            foreach (var notification in unreadNotifications)
            {
                notification.IsRead = true;
                unitOfWork.Repository<Notification>().Update(notification);
            }

            await unitOfWork.CommitAsync();

            logger.LogInformation("All notifications marked as read for user {UserId}", userId);
            return ResultResponse<NotificationResponse>.Success($"Marked {unreadNotifications.Count()} notifications as read");
        }

        public async Task<ResultResponse<NotificationResponse>> MarkAsReadAsync(Guid notificationId, string userId, CancellationToken cancellationToken)
        {
            var spec = new NotificiationSpecification(n => n.Id == notificationId && n.UserId == userId);
            var notification = await unitOfWork.Repository<Notification>()
                .GetByIdSpecTracked(spec, cancellationToken);

            if (notification is null)
            {
                logger.LogWarning("Notification {NotificationId} not found for user {UserId}", notificationId, userId);
                return ResultResponse<NotificationResponse>.Failure("Notification not found");
            }

            if (notification.IsRead)
            {
                return ResultResponse<NotificationResponse>.Success("Notification already marked as read");
            }

            notification.IsRead = true;
            unitOfWork.Repository<Notification>().Update(notification);
            await unitOfWork.CommitAsync();

            logger.LogInformation("Notification {NotificationId} marked as read", notificationId);
            return ResultResponse<NotificationResponse>.Success("Notification marked as read");
        }

        //Private Method
        private static string GenerateNotificationContent(NotificationType type)
        {
            return type switch
            {
                NotificationType.Like => "liked your post",
                NotificationType.Comment => "commented on your post",
                NotificationType.Follow => "started following you",
                NotificationType.FriendRequest => "sent you a friend request",
                NotificationType.Message => "sent you a message",
                _ => "sent you a notification"
            };
        }
    }
}

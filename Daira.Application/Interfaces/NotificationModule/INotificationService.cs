namespace Daira.Application.Interfaces.NotificationModule
{
    public interface INotificationService
    {
        // Get Notification Fro User
        Task<ResultResponse<NotificationResponse>> GetNotificationUserAsync(string userId, int pageNumber, int pageSize, CancellationToken cancellationToken = default);
        //Get Count UnRead
        Task<ResultResponse<int>> GetUnreadCountAsync(string userId, CancellationToken cancellationToken = default);
        //Mark Notifcation As read
        Task<ResultResponse<NotificationResponse>> MarkAsReadAsync(Guid NotificationId, string userId, CancellationToken cancellationToken = default);
        //Delete Notification
        Task<ResultResponse<NotificationResponse>> DeleteNotificationAsync(Guid NotificationId, string userId, CancellationToken cancellationToken = default);
        //Mark All As Read
        Task<ResultResponse<NotificationResponse>> MarkAllAsReadAsync(string userId, CancellationToken cancellationToken = default);
        //Create Notification
        Task<ResultResponse<NotificationResponse>> CreateNotification(CreateNotificationDto notificationDto, CancellationToken cancellationToken = default);



    }
}

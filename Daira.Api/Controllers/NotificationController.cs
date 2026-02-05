namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController(INotificationService notificationService, IConnectionService connectionService,
        IHubContext<ChatHub, IChatHubClient> hubContext) : ControllerBase
    {
        // Get paginated notifications for a user
        [HttpGet]
        [ProducesResponseType(typeof(ResultResponse<NotificationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetNotifications([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();

            var result = await notificationService.GetNotificationUserAsync(userId, pageNumber, pageSize);
            return Ok(result);
        }

        // Get count of unread notifications
        [HttpGet("unread-count")]
        [ProducesResponseType(typeof(ResultResponse<int>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUnreadCount()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();

            var result = await notificationService.GetUnreadCountAsync(userId);
            return Ok(result);
        }


        // Mark a specific notification as read
        [HttpPut("read/{id}")]
        [ProducesResponseType(typeof(ResultResponse<NotificationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkAsRead(Guid id, CancellationToken cancellationToken = default)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();

            var result = await notificationService.MarkAsReadAsync(id, userId, cancellationToken);
            if (!result.Succeeded) return BadRequest(result);

            var connections = await connectionService.GetConnectionsAsync(userId);
            if (connections.Any())
            {
                await hubContext.Clients.Clients(connections).NotificationRead(id);
            }

            return Ok(result);
        }

        // Mark all notifications as read
        [HttpPut("read-all")]
        [ProducesResponseType(typeof(ResultResponse<NotificationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();

            var result = await notificationService.MarkAllAsReadAsync(userId);

            // Send real-time update to client
            if (result.Succeeded)
            {
                var connections = await connectionService.GetConnectionsAsync(userId);
                if (connections.Any())
                {
                    await hubContext.Clients.Clients(connections).AllNotificationsRead();
                }
            }

            return Ok(result);
        }

        // Delete a notification
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ResultResponse<NotificationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();

            var result = await notificationService.DeleteNotificationAsync(id, userId);
            if (!result.Succeeded) return BadRequest(result);

            return Ok(result);
        }

        // Create a new notification and send it in real time
        [HttpPost]
        [ProducesResponseType(typeof(ResultResponse<NotificationResponse>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto request)
        {
            var actorId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (actorId is null) return Unauthorized();

            var result = await notificationService.CreateNotification(request);

            if (!result.Succeeded) return BadRequest(result);

            return CreatedAtAction(nameof(GetNotifications), result);
        }

    }
}

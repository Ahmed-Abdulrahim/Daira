namespace Daira.Api.Hub
{
    [Authorize]
    public class ChatHub(IConversationService conversationService, IMessageService messageService, IConnectionService connectionService,
            ILogger<ChatHub> logger) : Hub<IChatHubClient>
    {
        private string UserId => Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? throw new HubException("User ID not found in token");

        private string DisplayName => Context.User?.FindFirst(ClaimTypes.Name)?.Value
            ?? Context.User?.FindFirst("name")?.Value
            ?? "Unknown User";

        // Initalize once User Open App
        public override async Task OnConnectedAsync()
        {
            var userId = UserId;
            await connectionService.AddConnectionAsync(userId, Context.ConnectionId);
            logger.LogInformation("User {UserId} connected with ConnectionId {ConnectionId}", userId, Context.ConnectionId);

            var allConversation = await conversationService.GetUserConversationsAsync(UserId);
            if (allConversation.Succeeded && allConversation.ListOfData != null)
            {
                foreach (var conversation in allConversation.ListOfData)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, conversation.Id.ToString());
                }
            }
            await Clients.Others.UserOnline(userId);
            await base.OnConnectedAsync();
        }

        //Disconnect 
        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userId = UserId;
            await connectionService.RemoveConnectionAsync(userId, Context.ConnectionId);
            logger.LogInformation("User {UserId} disconnected. Exception: {Exception}", userId, exception?.Message);
            var userOnline = await connectionService.IsUserOnlineAsync(userId);
            if (!userOnline)
            {
                await Clients.Others.UserOffline(userId);
            }
            await base.OnDisconnectedAsync(exception);
        }

        //Retrive All Message From Group
        public async Task JoinConversation(Guid conversationId)
        {
            var userId = UserId;
            var existParticipant = await conversationService.IsUserInConversationAsync(conversationId, userId);
            if (!existParticipant)
            {
                throw new HubException("You are not a participant in this conversation");
            }
            await Groups.AddToGroupAsync(Context.ConnectionId, conversationId.ToString());
            logger.LogInformation("User {UserId} joined conversation {ConversationId}", userId, conversationId);

        }

        //Leave Conversation
        public async Task LeaveConversation(Guid conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId.ToString());
            logger.LogInformation("User {UserId} left conversation {ConversationId}", UserId, conversationId);

        }

        //Send Message
        public async Task SendMessage(Guid conversationId, string content)
        {
            var userId = UserId;

            var request = new SendMessageRequest
            {
                ConversationId = conversationId,
                Content = content
            };

            var result = await messageService.SendMessageAsync(request, userId);

            if (!result.Succeeded)
            {
                throw new HubException(result.Message ?? "Failed to send message");
            }

            await Clients.Group(conversationId.ToString()).ReceiveMessage(result.Data!);
            logger.LogInformation("User {UserId} sent message to conversation {ConversationId}", userId, conversationId);
        }

        //typing
        public async Task StartTyping(Guid conversationId)
        {
            var userId = UserId;
            var displayName = DisplayName;

            await Clients.OthersInGroup(conversationId.ToString()).UserTyping(conversationId, userId, displayName);
        }

        //Stoptyping
        public async Task StopTyping(Guid conversationId)
        {
            var userId = UserId;

            await Clients.OthersInGroup(conversationId.ToString()).UserStoppedTyping(conversationId, userId);
        }

        //Mark As Read
        public async Task MarkAsRead(Guid conversationId)
        {
            var userId = UserId;

            var result = await messageService.MarkConversationAsReadAsync(conversationId, userId);

            if (result.Succeeded)
            {
                // Notify others that messages have been read
                await Clients.OthersInGroup(conversationId.ToString()).MessagesRead(conversationId, userId);
            }
        }
    }
}

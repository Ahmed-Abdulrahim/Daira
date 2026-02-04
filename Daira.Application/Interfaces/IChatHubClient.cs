using Daira.Application.Response.ParticipantsMosule;

namespace Daira.Application.Interfaces
{
    public interface IChatHubClient
    {
        Task ReceiveMessage(MessageResponse message);
        Task UserJoinedConversation(Guid conversationId, ParticipantsResponse participant);
        Task UserLeftConversation(Guid conversationId, string userId);
        Task UserTyping(Guid conversationId, string userId, string displayName);
        Task UserStoppedTyping(Guid conversationId, string userId);
        Task UserOnline(string userId);
        Task UserOffline(string userId);
        Task MessagesRead(Guid conversationId, string userId);
        Task AddedToConversation(ConversationResponse conversation);
    }
}

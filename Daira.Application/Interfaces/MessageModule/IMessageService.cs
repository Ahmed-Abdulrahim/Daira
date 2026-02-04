namespace Daira.Application.Interfaces.MessageModule
{
    public interface IMessageService
    {
        //Get Message By Id
        Task<ResultResponse<MessageResponse>> GetByIdAsync(Guid messageId, string userId, CancellationToken cancellationToken = default);
        //Get All Messages For Conversation
        Task<ResultResponse<MessageResponse>> GetConversationMessagesAsync(Guid conversationId, string userId, int pageNumber = 1, int pageSize = 50, CancellationToken cancellationToken = default);
        // Send Message
        Task<ResultResponse<MessageResponse>> SendMessageAsync(SendMessageRequest request, string senderId, CancellationToken cancellationToken = default);
        // Mark Message As read
        Task<ResultResponse<MessageResponse>> MarkAsReadAsync(Guid messageId, string userId, CancellationToken cancellationToken = default);
        // Mark Conversation As Read
        Task<ResultResponse<MessageResponse>> MarkConversationAsReadAsync(Guid conversationId, string userId, CancellationToken cancellationToken = default);
        // Delete Message
        Task<ResultResponse<MessageResponse>> DeleteMessaegAsync(Guid messageId, string userId, CancellationToken cancellationToken = default);
    }
}

namespace Daira.Application.Interfaces.ConversationModule
{
    public interface IConversationService
    {
        //Get Conversation By Id
        Task<ResultResponse<ConversationResponse>> GetByIdAsync(Guid conversationId, string userId, CancellationToken cancellationToken = default);
        //Create Conversation
        Task<ResultResponse<ConversationResponse>> CreateAsync(CreateConversationRequest request, string creatorId, CancellationToken cancellationToken = default);
        //Check if user is in This Conversation
        Task<bool> IsUserInConversationAsync(Guid conversationId, string userId, CancellationToken cancellationToken = default);

        //Get All Conversation For User
        Task<ResultResponse<ConversationResponse>> GetUserConversationsAsync(string userId, CancellationToken cancellationToken = default);
        // Add Participant
        Task<ResultResponse<ConversationResponse>> AddParticipantAsync(Guid conversationId, string userId, string requestingUserId, CancellationToken cancellationToken = default);
        //RemoveParticipant
        Task<ResultResponse<ConversationResponse>> RemoveParticipantAsync(Guid conversationId, string userId, string requestingUserId, CancellationToken cancellationToken = default);

    }
}

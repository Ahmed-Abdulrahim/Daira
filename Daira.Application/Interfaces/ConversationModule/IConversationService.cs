using Daira.Application.Response.ConversationModule;

namespace Daira.Application.Interfaces.ConversationModule
{
    public interface IConversationService
    {
        Task<ResultResponse<ConversationResponse>> GetByIdAsync(Guid conversationId, string userId, CancellationToken cancellationToken = default);
        Task<ResultResponse<ConversationResponse>> CreateAsync(CreateConversationRequest request, string creatorId, CancellationToken cancellationToken = default);
        /* Task<Result<ParticipantDto>> AddParticipantAsync(Guid conversationId, string userId, string requestingUserId, CancellationToken cancellationToken = default);
        Task<Result> RemoveParticipantAsync(Guid conversationId, string userId, string requestingUserId, CancellationToken cancellationToken = default);
        Task<bool> IsUserInConversationAsync(Guid conversationId, string userId, CancellationToken cancellationToken = default);
        Task<Result<List<string>>> GetConversationParticipantIdsAsync(Guid conversationId, CancellationToken cancellationToken = default);
                Task<Result<List<ConversationListDto>>> GetUserConversationsAsync(string userId, CancellationToken cancellationToken = default);*/
    }
}

using Daira.Application.Response.ConversationModule;
using Microsoft.EntityFrameworkCore;

namespace Daira.Infrastructure.Services.ConversationService
{
    public class ConversationService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork, IMapper mapper, ILogger<ConversationService> logger) : IConversationService
    {
        //Create Conversation
        public async Task<ResultResponse<ConversationResponse>> CreateAsync(CreateConversationRequest request, string creatorId, CancellationToken cancellationToken = default)
        {
            //validate type
            if (!Enum.TryParse<ConversationType>(request.Type, true, out var conversationType))
            {
                logger.LogError("Conversation Type is not Direct or Group");
                return ResultResponse<ConversationResponse>.Failure("Direct conversations must have exactly one other participant");
            }
            if (conversationType == ConversationType.Direct && request.ParticipantIds.Count != 1)
            {
                logger.LogWarning("Direct conversations must have exactly one other participant");
                return ResultResponse<ConversationResponse>.Failure("Direct conversations must have exactly one other participant");
            }
            var conversation = new Conversation
            {
                Type = conversationType,
                Name = conversationType == ConversationType.Group ? request.Name : null,
                CreatedById = creatorId,
                CreatedAt = DateTime.UtcNow,
                Participants = new List<ConversationParticipant>(),
                Messages = new List<Message>()
            };
            conversation.Participants.Add(new ConversationParticipant
            {
                Id = Guid.NewGuid(),
                UserId = creatorId,
                JoinedAt = DateTime.UtcNow
            });

            // Add other participants
            foreach (var participantId in request.ParticipantIds)
            {
                if (participantId == creatorId) continue;

                var userExists = await userManager.FindByIdAsync(participantId);
                if (userExists is null)
                    return ResultResponse<ConversationResponse>.Failure($"User with ID '{participantId}' not found");

                conversation.Participants.Add(new ConversationParticipant
                {
                    Id = Guid.NewGuid(),
                    UserId = participantId,
                    JoinedAt = DateTime.UtcNow
                });
            }

            await unitOfWork.Repository<Conversation>().AddAsync(conversation);
            await unitOfWork.CommitAsync();

            // Reload with full details
            var spec = new ConversationSpecification(conversation.Id);
            var getAllData = await unitOfWork.Repository<Conversation>().GetByIdSpecTracked(spec);
            var map = mapper.Map<ConversationResponse>(getAllData);
            return ResultResponse<ConversationResponse>.Success(map);

        }


        //Get Conversation By Id
        public async Task<ResultResponse<ConversationResponse>> GetByIdAsync(Guid conversationId, string userId, CancellationToken cancellationToken = default)
        {
            var spec = new ConversationSpecification(c => c.Id == conversationId && c.Participants.Any(p => p.UserId == userId));
            var existConversation = await unitOfWork.Repository<Conversation>().GetByIdSpecTracked(spec, cancellationToken);
            if (existConversation is null)
            {
                logger.LogWarning("GetConversationByIdAsync {CoversationId} not Found With This User", conversationId);
                return ResultResponse<ConversationResponse>.Failure("Conversation not found or you don't have access");
            }

            var mapConversation = mapper.Map<ConversationResponse>(existConversation);
            return ResultResponse<ConversationResponse>.Success(mapConversation);
        }
    }
}

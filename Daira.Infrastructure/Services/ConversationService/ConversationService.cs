using Daira.Application.Response.ParticipantsMosule;

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

        //Get All Conversations For User
        public async Task<ResultResponse<ConversationResponse>> GetUserConversationsAsync(string userId, CancellationToken cancellationToken = default)
        {
            var spec = new ConversationSpecification(c => c.CreatedById == userId || c.Participants.Any(p => p.UserId == userId));
            var getAllConversation = await unitOfWork.Repository<Conversation>().GetAllWithSpec(spec, cancellationToken);
            if (!getAllConversation.Any())
            {
                logger.LogInformation("No Conversation Found");
                return ResultResponse<ConversationResponse>.Success("No Conversaion Found");
            }
            var map = mapper.Map<List<ConversationResponse>>(getAllConversation);
            return ResultResponse<ConversationResponse>.Success(map);
        }

        //Check if user Is In Conversation
        public async Task<bool> IsUserInConversationAsync(Guid conversationId, string userId, CancellationToken cancellationToken = default)
        {
            var spec = new ConversationSpecification(c => c.Id == conversationId && c.Participants.Any(c => c.UserId == userId));
            var getConversation = await unitOfWork.Repository<Conversation>().GetByIdSpecTracked(spec, cancellationToken);
            if (getConversation is null)
            {
                logger.LogInformation("User is not in this Conversation");
                return false;
            }
            return true;

        }

        //Add Participant
        public async Task<ResultResponse<ParticipantsResponse>> AddParticipantAsync(Guid conversationId, string userId, string requestingUserId, CancellationToken cancellationToken = default)
        {
            var spec = new ConversationSpecification(c => c.Id == conversationId);
            var getConversation = await unitOfWork.Repository<Conversation>().GetByIdSpecTracked(spec, cancellationToken);
            if (getConversation is null)
            {
                logger.LogWarning("Conversation is not Found");
                return ResultResponse<ParticipantsResponse>.Failure("Conversation Not Found");
            }
            if (getConversation.Type == ConversationType.Direct)
            {
                logger.LogWarning("Cannot Add participants to Direct Chat");
                return ResultResponse<ParticipantsResponse>.Failure("Cannot Add participants to Direct Chat");
            }
            var paricipant = await IsUserInConversationAsync(getConversation.Id, requestingUserId, cancellationToken);
            if (!paricipant)
            {
                logger.LogWarning("You are not a participant in this conversation");
                return ResultResponse<ParticipantsResponse>.Failure("You are not a participant in this conversation");
            }
            var existUser = await userManager.FindByIdAsync(userId);
            if (existUser is null)
            {
                logger.LogWarning("User Not Found");
                return ResultResponse<ParticipantsResponse>.Failure("User not Found");
            }
            var addParticipants = await AddParticipantInConversation(conversationId, userId, cancellationToken);
            if (!addParticipants)
            {
                logger.LogWarning("User is already a participant in this conversation");
                return ResultResponse<ParticipantsResponse>.Failure("User is already a participant in this conversation");
            }
            var map = mapper.Map<ParticipantsResponse>(addParticipants);
            return ResultResponse<ParticipantsResponse>.Success(map);
        }

        //RemoveParticipant
        public async Task<ResultResponse<ConversationResponse>> RemoveParticipantAsync(Guid conversationId, string userId, string requestingUserId, CancellationToken cancellationToken = default)
        {
            var spec = new ConversationSpecification(c => c.Id == conversationId);
            var existConversation = await unitOfWork.Repository<Conversation>().GetByIdSpecTracked(spec, cancellationToken);
            if (existConversation is null)
            {
                logger.LogWarning("Conversation not found");
                return ResultResponse<ConversationResponse>.Failure("Conversation not found");
            }
            if (existConversation.Type == ConversationType.Direct)
            {
                logger.LogWarning("Cannot remove participants from a direct conversation");
                return ResultResponse<ConversationResponse>.Failure("Cannot remove participants from a direct conversation");
            }
            var paricipant = await IsUserInConversationAsync(existConversation.Id, requestingUserId, cancellationToken);
            if (!paricipant)
            {
                logger.LogWarning("You are not a participant in this conversation");
                return ResultResponse<ConversationResponse>.Failure("You are not a participant in this conversation");
            }
            var checkSpec = new ConversationParticipantSpecification(c => c.ConversationId == conversationId && c.UserId == userId);
            var checkParticipants = await unitOfWork.Repository<ConversationParticipant>().GetByIdSpecTracked(checkSpec, cancellationToken);
            if (checkParticipants is null)
            {
                logger.LogWarning("User is not a participant in this conversation");
                return ResultResponse<ConversationResponse>.Failure("User is not a participant in this conversation");
            }
            unitOfWork.Repository<ConversationParticipant>().Delete(checkParticipants);
            await unitOfWork.CommitAsync();
            return ResultResponse<ConversationResponse>.Success();

        }
        //Privat Method
        private async Task<bool> AddParticipantInConversation(Guid conversationId, string userId, CancellationToken cancellationToken)
        {
            var spec = new ConversationSpecification(c => c.Id == conversationId && c.Participants.Any(p => p.UserId == userId));
            var checkParticipants = await unitOfWork.Repository<Conversation>().GetByIdSpecTracked(spec, cancellationToken);
            if (checkParticipants is not null) return false;
            var addParticipants = new ConversationParticipant
            {
                Id = Guid.NewGuid(),
                ConversationId = conversationId,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            };
            await unitOfWork.Repository<ConversationParticipant>().AddAsync(addParticipants);
            await unitOfWork.CommitAsync();
            return true;
        }


    }
}

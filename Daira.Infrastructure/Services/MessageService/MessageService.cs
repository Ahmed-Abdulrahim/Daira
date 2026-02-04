namespace Daira.Infrastructure.Services.MessageService
{
    public class MessageService(IMapper mapper, IConversationService conversationService, IUnitOfWork unitOfWork, ILogger<MessageService> logger) : IMessageService
    {

        //GetById
        public async Task<ResultResponse<MessageResponse>> GetByIdAsync(Guid messageId, string userId, CancellationToken cancellationToken = default)
        {
            var spec = new MessageSpecification(m => m.Id == messageId);
            var existMessage = await unitOfWork.Repository<Message>().GetByIdSpecTracked(spec, cancellationToken);
            if (existMessage is null)
            {
                logger.LogWarning("Message not Found");
                return ResultResponse<MessageResponse>.Failure("Mesasge not Found");
            }
            var existParticipant = await conversationService.IsUserInConversationAsync(existMessage.ConversationId, userId);
            if (!existParticipant)
            {
                logger.LogWarning("You are not a participant in this conversation");
                return ResultResponse<MessageResponse>.Failure("You are not a participant in this conversation");
            }
            var map = mapper.Map<MessageResponse>(existMessage);
            return ResultResponse<MessageResponse>.Success(map);
        }

        //GetAll Messages
        public async Task<ResultResponse<MessageResponse>> GetConversationMessagesAsync(Guid conversationId, string userId, int pageNumber = 1, int pageSize = 50, CancellationToken cancellationToken = default)
        {
            var existUserInConversation = await conversationService.IsUserInConversationAsync(conversationId, userId);
            if (!existUserInConversation)
            {
                logger.LogWarning("You are not a participant in this conversation");
                return ResultResponse<MessageResponse>.Failure("You are not a participant in this conversation");
            }
            var spec = new MessageSpecification(m => m.ConversationId == conversationId);
            var getAllMessages = await unitOfWork.Repository<Message>().GetAllWithSpec(spec, cancellationToken);
            if (!getAllMessages.Any())
            {
                logger.LogWarning("No Message Found");
                return ResultResponse<MessageResponse>.Failure("No Message Found");
            }
            var map = mapper.Map<List<MessageResponse>>(getAllMessages);
            return ResultResponse<MessageResponse>.Success(map);
        }

        //Mark As Reaed
        public async Task<ResultResponse<MessageResponse>> MarkAsReadAsync(Guid messageId, string userId, CancellationToken cancellationToken = default)
        {
            var spec = new MessageSpecification(m => m.Id == messageId);
            var existMessage = await unitOfWork.Repository<Message>().GetByIdSpecTracked(spec);
            if (existMessage is null)
            {
                logger.LogWarning("Message not Found");
                return ResultResponse<MessageResponse>.Failure("Message  not Found");
            }
            var existUserInConversation = await conversationService.IsUserInConversationAsync(existMessage.ConversationId, userId);
            if (!existUserInConversation)
            {
                logger.LogWarning("You are not a participant in this conversation");
                return ResultResponse<MessageResponse>.Failure("You are not a participant in this conversation");
            }
            existMessage.IsRead = true;

            unitOfWork.Repository<Message>().Update(existMessage);
            await unitOfWork.CommitAsync();
            return ResultResponse<MessageResponse>.Success("Messgae Mark As Read Successfully");
        }

        // Mark Conversation As Read
        public async Task<ResultResponse<MessageResponse>> MarkConversationAsReadAsync(Guid conversationId, string userId, CancellationToken cancellationToken = default)
        {
            var existUserInConversation = await conversationService.IsUserInConversationAsync(conversationId, userId, cancellationToken);
            if (!existUserInConversation)
            {
                logger.LogWarning("You are not a participant in this conversation");
                return ResultResponse<MessageResponse>.Failure("You are not a participant in this conversation");
            }
            var spec = new MessageSpecification(m => m.ConversationId == conversationId && m.SenderId != userId && !m.IsRead);
            var unreadMessages = await unitOfWork.Repository<Message>().GetAllWithSpec(spec, cancellationToken);
            if (!unreadMessages.Any())
            {
                logger.LogWarning("There is no unRead Message");
                return ResultResponse<MessageResponse>.Failure("There is no unRead Message");
            }
            foreach (var message in unreadMessages)
            {
                message.IsRead = true;
                unitOfWork.Repository<Message>().Update(message);
            }
            await unitOfWork.CommitAsync();
            return ResultResponse<MessageResponse>.Success("Conversation Mark As Read ");

        }

        //Send Message
        public async Task<ResultResponse<MessageResponse>> SendMessageAsync(SendMessageRequest request, string senderId, CancellationToken cancellationToken = default)
        {
            var existUserInConversation = await conversationService.IsUserInConversationAsync(request.ConversationId, senderId, cancellationToken);
            if (!existUserInConversation)
            {
                logger.LogWarning("You are not a participant in this conversation");
                return ResultResponse<MessageResponse>.Failure("You are not a participant in this conversation");
            }
            var addMessages = new Message
            {
                Content = request.Content,
                ConversationId = request.ConversationId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                SenderId = senderId
            };
            await unitOfWork.Repository<Message>().AddAsync(addMessages);
            await unitOfWork.CommitAsync();
            var map = mapper.Map<MessageResponse>(addMessages);
            return ResultResponse<MessageResponse>.Success(map);
        }

        //Delete Message
        public async Task<ResultResponse<MessageResponse>> DeleteMessaegAsync(Guid messageId, string userId, CancellationToken cancellationToken = default)
        {
            var spec = new MessageSpecification(m => m.Id == messageId && m.SenderId == userId);
            var existMessage = await unitOfWork.Repository<Message>().GetByIdSpecTracked(spec, cancellationToken);
            if (existMessage is null)
            {
                logger.LogWarning("Message not Found");
                return ResultResponse<MessageResponse>.Failure("Mesasge not Found");
            }
            var existUserInConversation = await conversationService.IsUserInConversationAsync(existMessage.ConversationId, userId, cancellationToken);
            if (!existUserInConversation)
            {
                logger.LogWarning("You are not a participant in this conversation");
                return ResultResponse<MessageResponse>.Failure("You are not a participant in this conversation");
            }

            unitOfWork.Repository<Message>().Delete(existMessage);
            await unitOfWork.CommitAsync();
            return ResultResponse<MessageResponse>.Success("Message Deleted Successfully");
        }

    }
}

namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConversationController(IConversationService conversationService, IHubContext<ChatHub, IChatHubClient> hubContext, IConnectionService connectionService) : ControllerBase
    {
        //CreateConversation
        [HttpPost("create-conversation")]
        [ProducesResponseType(typeof(ResultResponse<ConversationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateConversation(CreateConversationRequest createConversationRequest)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var result = await conversationService.CreateAsync(createConversationRequest, userId);
            if (!result.Succeeded) return BadRequest(result);
            foreach (var participant in result.Data!.Participants)
            {
                if (participant.UserId != userId)
                {
                    var connections = await connectionService.GetConnectionsAsync(participant.UserId);
                    foreach (var connectionId in connections)
                    {
                        await hubContext.Clients.Client(connectionId).AddedToConversation(result.Data);
                    }
                }
            }
            return Ok(result);
        }

        //get Conversation
        [HttpGet("get-conversation/{id}")]
        [ProducesResponseType(typeof(ResultResponse<ConversationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetConversation(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var result = await conversationService.GetByIdAsync(id, userId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //get All Conversation For One User
        [HttpGet("get-User-conversation")]
        [ProducesResponseType(typeof(ResultResponse<List<ConversationResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllUserConversations()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user is null) return Unauthorized();
            var result = await conversationService.GetUserConversationsAsync(user);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        // Add Paticipants
        [HttpPost("add-participant/{userId}-to-conversation{id}")]
        [ProducesResponseType(typeof(ResultResponse<List<ConversationResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddParticipant(Guid id, string userId)
        {
            var requestId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (requestId is null) return Unauthorized();
            var result = await conversationService.AddParticipantAsync(id, requestId, userId);
            if (!result.Succeeded) return BadRequest(result);
            await hubContext.Clients.Group(id.ToString()).UserJoinedConversation(id, result.Data!);

            // Notify the new participant about being added
            var connections = await connectionService.GetConnectionsAsync(userId);
            if (connections.Any())
            {
                var conversationResult = await conversationService.GetByIdAsync(id, userId);
                if (conversationResult.Succeeded)
                {
                    foreach (var connectionId in connections)
                    {
                        await hubContext.Clients.Client(connectionId).AddedToConversation(conversationResult.Data!);
                    }
                }
            }
            return Ok(result);
        }

        // Remove Paticipants
        [HttpDelete("remove-participant/{userId}-to-conversation{id}")]
        [ProducesResponseType(typeof(ResultResponse<List<ConversationResponse>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveParticipant(Guid id, string userId)
        {
            var requestId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (requestId is null) return Unauthorized();
            var result = await conversationService.RemoveParticipantAsync(id, requestId, userId);
            if (!result.Succeeded) return BadRequest(result);
            await hubContext.Clients.Group(id.ToString()).UserLeftConversation(id, userId);
            return Ok(result);
        }


    }
}

namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MessageController(IMessageService messageService, IHubContext<ChatHub, IChatHubClient> hubContext) : ControllerBase
    {
        // Get Message By Id
        [HttpPost("get-message/{id}")]
        [ProducesResponseType(typeof(ResultResponse<MessageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMessageById(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var result = await messageService.GetByIdAsync(id, userId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        // Get All Messages in Conversation
        [HttpGet("get-all-messages/{conversationId}")]
        [ProducesResponseType(typeof(ResultResponse<MessageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMessagesInConversation(Guid conversationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var result = await messageService.GetConversationMessagesAsync(conversationId, userId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        // Send Message
        [HttpPost("send-Message")]
        [ProducesResponseType(typeof(ResultResponse<MessageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendMessage(SendMessageRequest sendMessage)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var result = await messageService.SendMessageAsync(sendMessage, userId);
            if (!result.Succeeded) return BadRequest(result);
            await hubContext.Clients.Group(sendMessage.ConversationId.ToString()).ReceiveMessage(result.Data!);

            return Ok(result);
        }

        //Mark As Read
        [HttpPut("read-message/{id}")]
        [ProducesResponseType(typeof(ResultResponse<MessageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ReadMessage(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var result = await messageService.MarkAsReadAsync(id, userId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //Mark conversation  As Read
        [HttpPut("read-conversation/{id}")]
        [ProducesResponseType(typeof(ResultResponse<MessageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> MarkConversationAsRead(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var result = await messageService.MarkConversationAsReadAsync(id, userId);
            if (!result.Succeeded) return BadRequest(result);
            await hubContext.Clients.Group(id.ToString()).MessagesRead(id, userId);
            return Ok(result);
        }

        //Delete Message
        [HttpDelete("delete-message/{id}")]
        [ProducesResponseType(typeof(ResultResponse<MessageResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMessage(Guid id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) return Unauthorized();
            var result = await messageService.DeleteMessaegAsync(id, userId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }
    }
}

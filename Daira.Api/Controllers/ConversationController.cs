using Daira.Application.DTOs.ConversationModule;
using Daira.Application.Interfaces.ConversationModule;
using Daira.Application.Response.ConversationModule;

namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ConversationController(IConversationService conversationService) : ControllerBase
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

    }
}

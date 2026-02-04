namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FriendshipController(IFriendShipService friendShipService) : ControllerBase
    {
        //GetAll  Friend Request 
        [HttpGet("GetAll-friendRequest")]
        [ProducesResponseType(typeof(ResultResponse<FriendshipResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFriendShip()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            var result = await friendShipService.GetAllFriendRequests(userId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //GetAll  Friends 
        [HttpGet("GetAll-friends")]
        [ProducesResponseType(typeof(ResultResponse<FriendshipResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllFriends()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null) return Unauthorized();
            var result = await friendShipService.GetAllFriends(userId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //Send FriendRequest
        [HttpPost("request-friendship/{addresseeId}")]
        [ProducesResponseType(typeof(ResultResponse<FriendshipResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendFriendRequest(string addresseeId)
        {
            var requestId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (requestId is null) return Unauthorized();
            var result = await friendShipService.SendFriendRequestAsync(requestId, addresseeId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //Accept FriendRequest
        [HttpPut("accept-friendship/{id}")]
        [ProducesResponseType(typeof(ResultResponse<FriendshipResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AcceptFriendShip(Guid id)
        {
            var result = await friendShipService.AcceptFriendRequest(id);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //Decline Friend Request 
        [HttpPut("decline-friendship/{id}")]
        [ProducesResponseType(typeof(ResultResponse<FriendshipResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeclineFriendRequest(Guid id)
        {
            var result = await friendShipService.DeclineFriendRequest(id);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //UnFriend Friend Request 
        [HttpPost("unFriend/{id}")]
        [ProducesResponseType(typeof(ResultResponse<FriendshipResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnFriend(Guid id)
        {
            var reult = await friendShipService.UnFriend(id);
            if (!reult.Succeeded) return BadRequest(reult);
            return Ok(reult);
        }

    }
}

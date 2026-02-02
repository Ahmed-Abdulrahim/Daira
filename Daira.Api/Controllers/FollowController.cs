namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FollowController(IFollowService followService) : ControllerBase
    {
        //Follow User
        [HttpPost("follow-user/{id}")]
        [ProducesResponseType(typeof(FollowerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> FollowUser(string id)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user is null)
            {
                return Unauthorized();
            }
            var result = await followService.FollowUserAsync(user, id);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //UnFollow User
        [HttpPost("unfollow-user/{id}")]
        [ProducesResponseType(typeof(FollowerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnFollowUser(string id)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user is null)
            {
                return Unauthorized();
            }
            var result = await followService.UnFollowUserAsync(user, id);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //Get All Followers of a User
        [HttpGet("get-followers")]
        [ProducesResponseType(typeof(FollowerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFollowers()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId is null)
            {
                return Unauthorized();
            }
            var result = await followService.GetFollowersAsync(UserId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //Get All Following of a User
        [HttpGet("get-following")]
        [ProducesResponseType(typeof(FollowerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFollowing()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId is null)
            {
                return Unauthorized();
            }
            var result = await followService.GetFollowingAsync(UserId);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }
    }
}

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
    }
}

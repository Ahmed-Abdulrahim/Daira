namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostController(IPostService postService) : ControllerBase
    {
        [HttpPost("create-post")]

        [ProducesResponseType(typeof(CreatePostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<CreatePostResponse>> CreatePost(CreatePostDto createPostDto)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user is null)
            {
                return Unauthorized();
            }
            var result = await postService.CreatePostAsync(user, createPostDto);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);


        }
    }
}

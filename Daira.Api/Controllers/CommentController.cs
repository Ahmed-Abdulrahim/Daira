namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController(ICommentService commentService) : ControllerBase
    {
        [HttpPost("add-comment/{postId}")]
        [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddComment(Guid postId, [FromBody] AddCommentDto addCommentDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await commentService.AddComment(userId!, postId, addCommentDto);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}

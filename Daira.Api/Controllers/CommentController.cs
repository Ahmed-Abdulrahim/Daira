namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CommentController(ICommentService commentService) : ControllerBase
    {
        //Get Comments by Post Id
        [HttpGet("get-comments/{postId}")]
        [ProducesResponseType(typeof(IEnumerable<CommentResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCommentsByPostId(Guid postId)
        {
            var result = await commentService.GetCommentsByPostId(postId);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        //Add Comment
        [HttpPost("add-comment/{id}")]
        [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddComment(Guid id, [FromBody] AddCommentDto addCommentDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var result = await commentService.AddComment(userId!, id, addCommentDto);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //Delete Comment
        [HttpDelete("delete-comment/{id}")]
        [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();
            var result = await commentService.DeleteComment(userId, id);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        //Update Comment 
        [HttpPut("update-commnet/{id}")]
        [ProducesResponseType(typeof(CommentResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateComment(Guid id, [FromBody] AddCommentDto updateCommentDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId is null) return Unauthorized();
            var result = await commentService.UpdateComment(userId, id, updateCommentDto);
            if (!result.Succeeded)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}

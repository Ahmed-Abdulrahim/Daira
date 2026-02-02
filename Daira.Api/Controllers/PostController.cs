using Daira.Application.Response.LikeModule;

namespace Daira.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PostController(IPostService postService) : ControllerBase
    {
        // Create Post
        [HttpPost("create-post")]
        [ProducesResponseType(typeof(CreatePostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePost(CreatePostDto createPostDto)
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

        //GetById
        [HttpGet("get-post/{id}")]
        [ProducesResponseType(typeof(PostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await postService.GetPostByIdAsync(id);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //update Post
        [HttpPut("update-post/{id}")]
        [ProducesResponseType(typeof(UpdateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePost(Guid id, UpdatePostDto updatePostDto)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user is null)
            {
                return Unauthorized();
            }
            var result = await postService.UpdatePostAsync(id, user, updatePostDto);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //Delete Post
        [HttpDelete("delete-post/{id}")]
        [ProducesResponseType(typeof(PostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user is null)
            {
                return Unauthorized();
            }
            var result = await postService.DeletePostAsync(user, id);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);

        }

        //GetPosts for specific user
        [HttpGet("get-posts")]
        [ProducesResponseType(typeof(PostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPostsForSpecificUser()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user is null)
            {
                return Unauthorized();
            }
            var result = await postService.GetAllPostForSpecificUser(user);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        // Get Feed (Posts from followed users)
        [HttpGet("feed")]
        [ProducesResponseType(typeof(PostResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetFeed([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user is null)
            {
                return Unauthorized();
            }
            var result = await postService.GetFeedAsync(user, pageNumber, pageSize);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //Like Post
        [HttpPost("like-post/{id}")]
        [ProducesResponseType(typeof(LikeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> LikePost(Guid id)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user is null)
            {
                return Unauthorized();
            }
            var result = await postService.LikePostAsync(user, id);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //UnLike Post
        [HttpDelete("Unlike-post/{id}")]
        [ProducesResponseType(typeof(LikeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UnLikePost(Guid id)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user is null)
            {
                return Unauthorized();
            }
            var result = await postService.UnLikePostAsync(user, id);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

        //Get Post Likes
        [HttpGet("post-likes/{id}")]
        [ProducesResponseType(typeof(LikeResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetPostLikes(Guid id)
        {
            var result = await postService.GetPostLikesAsync(id);
            if (!result.Succeeded) return BadRequest(result);
            return Ok(result);
        }

    }
}
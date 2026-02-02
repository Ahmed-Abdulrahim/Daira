using Org.BouncyCastle.Bcpg;

namespace Daira.Infrastructure.Services.PostService
{
    public class PostService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostService> logger) : IPostService
    {
        public async Task<CreatePostResponse> CreatePostAsync(string userId, CreatePostDto dto)
        {
            try
            {
                logger.LogInformation("Creating post for user {UserId}", userId);
                var post = new Post
                {
                    Content = dto.Content,
                    ImageUrl = dto.ImageUrl,
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                await unitOfWork.Repository<Post>().AddAsync(post);
                await unitOfWork.CommitAsync();
                var spec = new PostSpecification(post.Id);
                var createdPost = await unitOfWork.Repository<Post>().GetByIdSpecTracked(spec);
                if (createdPost is null)
                {
                    logger.LogError("Failed to retrieve created post {PostId}", post.Id);
                    return CreatePostResponse.Failure("Post could not be retrieved after creation.");
                }
                var postDto = mapper.Map<CreatePostResponse>(createdPost);
                logger.LogInformation("Post {PostId} created successfully", post.Id);

                return CreatePostResponse.Success(postDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating post for user {UserId}", userId);
                return CreatePostResponse.Failure("An unexpected error occurred while creating the post.");
            }
        }

        public async Task<PostResponse> DeletePostAsync(string usreId, Guid PostId)
        {
            var spec = new PostSpecification(PostId);
            var getPost = await unitOfWork.Repository<Post>().GetByIdSpecTracked(spec);
            if (getPost is null) return PostResponse.Failure("Post not found ");
            if (getPost.User.Id != usreId) return PostResponse.Failure("Unauthorized to delete this post ");
            unitOfWork.Repository<Post>().Delete(getPost);
            await unitOfWork.CommitAsync();
            return PostResponse.Success("Post deleted successfully ");
        }

        public async Task<PostResponse> GetPostByIdAsync(Guid postId)
        {
            var spec = new PostSpecification(postId);
            var getPost = await unitOfWork.Repository<Post>().GetByIdSpecTracked(spec);
            if (getPost is null)
            {
                logger.LogWarning("Post {PostId} not found", postId);
                return PostResponse.Failure("Post not found ");
            }
            var postDto = mapper.Map<PostResponse>(getPost);
            if (postDto is null)
            {
                logger.LogError("Mapping failed for post {PostId}", postId);
                return PostResponse.Failure("Failed to map post data ");
            }
            return PostResponse.Success(postDto);
        }

        public async Task<UpdateResponse> UpdatePostAsync(Guid postId, string userId, UpdatePostDto updatePostDto)
        {
            var spec = new PostSpecification(postId);
            var getPost = await unitOfWork.Repository<Post>().GetByIdSpecTracked(spec);
            if (getPost is null)
            {
                logger.LogWarning("Post {PostId} not found for update", postId);
                return UpdateResponse.Failure("Post not found ");
            }
            if (getPost.User.Id != userId)
            {
                logger.LogWarning("User {UserId} unauthorized to update post {PostId}", userId, postId);
                return UpdateResponse.Failure("Unauthorized to update this post ");
            }
            getPost.Content = updatePostDto.Content ?? getPost.Content;
            getPost.ImageUrl = updatePostDto.ImageUrl ?? getPost.ImageUrl;
            getPost.UpdatedAt = DateTime.UtcNow;
            await unitOfWork.CommitAsync();
            var mapPost = mapper.Map<UpdateResponse>(getPost);
            return UpdateResponse.Success(mapPost);

        }
    }
}

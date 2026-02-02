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
    }
}

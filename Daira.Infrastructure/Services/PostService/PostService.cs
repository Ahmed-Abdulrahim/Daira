namespace Daira.Infrastructure.Services.PostService
{
    public class PostService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostService> logger) : IPostService
    {
        // Create Post
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

        // Delete Post
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

        // Get All Posts for Specific User
        public async Task<PostResponse> GetAllPostForSpecificUser(string userId)
        {
            var spec = new PostSpecification(r => r.User.Id == userId);
            var getPosts = await unitOfWork.Repository<Post>().GetAllWithSpec(spec);
            if (!getPosts.Any()) return PostResponse.Failure("No posts found for this user ");
            var postDtos = mapper.Map<List<GetPostResponse>>(getPosts);
            return PostResponse.Success(postDtos);

        }

        // Get Post By Id
        public async Task<PostResponse> GetPostByIdAsync(Guid postId)
        {
            var spec = new PostSpecification(postId);
            var getPost = await unitOfWork.Repository<Post>().GetByIdSpecTracked(spec);
            if (getPost is null)
            {
                logger.LogWarning("Post {PostId} not found", postId);
                return PostResponse.Failure("Post not found ");
            }
            var postDto = mapper.Map<GetPostResponse>(getPost);
            if (postDto is null)
            {
                logger.LogError("Mapping failed for post {PostId}", postId);
                return PostResponse.Failure("Failed to map post data ");
            }
            return PostResponse.Success(postDto);
        }

        // Update Post
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

        // Get Feed for User
        public async Task<PostResponse> GetFeedAsync(string userId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                logger.LogInformation("Getting feed for user {UserId}, page {PageNumber}, size {PageSize}", userId, pageNumber, pageSize);

                var followedUsers = await unitOfWork.Repository<Follower>()
                    .FindBy(f => f.FollowerId == userId)
                    .Select(f => f.FollowingId)
                    .ToListAsync();

                if (!followedUsers.Any())
                {
                    logger.LogInformation("User {UserId} is not following anyone", userId);
                    return PostResponse.Failure("You are not following anyone. Follow users to see their posts in your feed.");
                }

                var feedSpec = new FeedSpecification(followedUsers, pageNumber, pageSize);
                var posts = await unitOfWork.Repository<Post>().GetAllWithSpec(feedSpec);

                if (!posts.Any())
                {
                    logger.LogInformation("No posts found in feed for user {UserId}", userId);
                    return PostResponse.Failure("No posts found in your feed.");
                }

                var postDtos = mapper.Map<List<GetPostResponse>>(posts);
                logger.LogInformation("Retrieved {Count} posts for user {UserId}'s feed", postDtos.Count, userId);

                return PostResponse.Success(postDtos, "Feed retrieved successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting feed for user {UserId}", userId);
                return PostResponse.Failure("An unexpected error occurred while retrieving your feed.");
            }
        }

    }
}

namespace Daira.Infrastructure.Services.PostService
{
    public class PostService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<PostService> logger) : IPostService
    {
        // Create Post
        public async Task<ResultResponse<PostResponse>> CreatePostAsync(string userId, CreatePostDto dto)
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
                    return ResultResponse<PostResponse>.Failure("Post could not be retrieved after creation.");
                }
                var postDto = mapper.Map<PostResponse>(createdPost);
                logger.LogInformation("Post {PostId} created successfully", post.Id);

                return ResultResponse<PostResponse>.Success(postDto);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error creating post for user {UserId}", userId);
                return ResultResponse<PostResponse>.Failure("An unexpected error occurred while creating the post.");
            }
        }

        // Delete Post
        public async Task<ResultResponse<PostResponse>> DeletePostAsync(string usreId, Guid PostId)
        {
            var spec = new PostSpecification(PostId);
            var getPost = await unitOfWork.Repository<Post>().GetByIdSpecTracked(spec);
            if (getPost is null) return ResultResponse<PostResponse>.Failure("Post not found ");
            if (getPost.User.Id != usreId) return ResultResponse<PostResponse>.Failure("Unauthorized to delete this post ");
            unitOfWork.Repository<Post>().Delete(getPost);
            await unitOfWork.CommitAsync();
            return ResultResponse<PostResponse>.Success("Post deleted successfully ");
        }

        // Get All Posts for Specific User
        public async Task<ResultResponse<PostResponse>> GetAllPostForSpecificUser(string userId)
        {
            var spec = new PostSpecification(r => r.User.Id == userId);
            var getPosts = await unitOfWork.Repository<Post>().GetAllWithSpec(spec);
            if (!getPosts.Any()) return ResultResponse<PostResponse>.Failure("No posts found for this user ");
            var postDtos = mapper.Map<List<PostResponse>>(getPosts);
            return ResultResponse<PostResponse>.Success(postDtos);

        }

        // Get Post By Id
        public async Task<ResultResponse<PostResponse>> GetPostByIdAsync(Guid postId)
        {
            var spec = new PostSpecification(postId);
            var getPost = await unitOfWork.Repository<Post>().GetByIdSpecTracked(spec);
            if (getPost is null)
            {
                logger.LogWarning("Post {PostId} not found", postId);
                return ResultResponse<PostResponse>.Failure("Post not found ");
            }
            var postDto = mapper.Map<PostResponse>(getPost);
            if (postDto is null)
            {
                logger.LogError("Mapping failed for post {PostId}", postId);
                return ResultResponse<PostResponse>.Failure("Failed to map post data ");
            }
            return ResultResponse<PostResponse>.Success(postDto);
        }

        // Update Post
        public async Task<ResultResponse<PostResponse>> UpdatePostAsync(Guid postId, string userId, UpdatePostDto updatePostDto)
        {
            var spec = new PostSpecification(postId);
            var getPost = await unitOfWork.Repository<Post>().GetByIdSpecTracked(spec);
            if (getPost is null)
            {
                logger.LogWarning("Post {PostId} not found for update", postId);
                return ResultResponse<PostResponse>.Failure("Post not found ");
            }
            if (getPost.User.Id != userId)
            {
                logger.LogWarning("User {UserId} unauthorized to update post {PostId}", userId, postId);
                return ResultResponse<PostResponse>.Failure("Unauthorized to update this post ");
            }
            getPost.Content = updatePostDto.Content ?? getPost.Content;
            getPost.ImageUrl = updatePostDto.ImageUrl ?? getPost.ImageUrl;
            getPost.UpdatedAt = DateTime.UtcNow;
            await unitOfWork.CommitAsync();
            var mapPost = mapper.Map<PostResponse>(getPost);
            return ResultResponse<PostResponse>.Success(mapPost);

        }

        // Get Feed for User
        public async Task<ResultResponse<PostResponse>> GetFeedAsync(string userId, int pageNumber = 1, int pageSize = 10)
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
                    return ResultResponse<PostResponse>.Failure("You are not following anyone. Follow users to see their posts in your feed.");
                }

                var feedSpec = new FeedSpecification(followedUsers, pageNumber, pageSize);
                var posts = await unitOfWork.Repository<Post>().GetAllWithSpec(feedSpec);

                if (!posts.Any())
                {
                    logger.LogInformation("No posts found in feed for user {UserId}", userId);
                    return ResultResponse<PostResponse>.Failure("No posts found in your feed.");
                }

                var postDtos = mapper.Map<List<PostResponse>>(posts);
                logger.LogInformation("Retrieved {Count} posts for user {UserId}'s feed", postDtos.Count, userId);

                return ResultResponse<PostResponse>.Success(postDtos, "Feed retrieved successfully.");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error getting feed for user {UserId}", userId);
                return ResultResponse<PostResponse>.Failure("An unexpected error occurred while retrieving your feed.");
            }
        }

        //Like Post
        public async Task<ResultResponse<LikeResponse>> LikePostAsync(string userId, Guid postId)
        {
            var existPost = await unitOfWork.Repository<Post>().GetByIdAsync(postId);
            if (existPost is null)
            {
                logger.LogWarning("Post {PostId} not found for liking", postId);
                return ResultResponse<LikeResponse>.Failure("Post not found ");
            }
            var spec = new LikeSpecification(l => l.UserId == userId && l.PostId == postId);
            var existLike = await unitOfWork.Repository<Like>().GetByIdSpec(spec);
            if (existLike is not null)
            {
                logger.LogInformation("User {UserId} already liked post {PostId}", userId, postId);
                return ResultResponse<LikeResponse>.Failure("You have already liked this post ");
            }
            existPost.LikesCount += 1;
            var like = new Like
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            await unitOfWork.Repository<Like>().AddAsync(like);
            await unitOfWork.CommitAsync();
            var likeDto = mapper.Map<LikeResponse>(like);
            logger.LogInformation("User {UserId} liked post {PostId} successfully", userId, postId);
            return ResultResponse<LikeResponse>.Success(likeDto, "Post liked successfully ");
        }

        //Unlike Post
        public async Task<ResultResponse<LikeResponse>> UnLikePostAsync(string userId, Guid postId)
        {
            var existPost = await unitOfWork.Repository<Post>().GetByIdAsync(postId);
            if (existPost is null)
            {
                logger.LogWarning("Post {PostId} not found for unliking", postId);
                return ResultResponse<LikeResponse>.Failure("Post not found ");
            }
            var spec = new LikeSpecification(l => l.UserId == userId && l.PostId == postId);
            var existLike = await unitOfWork.Repository<Like>().GetByIdSpec(spec);
            if (existLike is null)
            {
                logger.LogInformation("User {UserId} has not liked post {PostId}", userId, postId);
                return ResultResponse<LikeResponse>.Failure("You have not liked this post ");
            }
            existPost.LikesCount -= 1;
            unitOfWork.Repository<Like>().Delete(existLike);
            await unitOfWork.CommitAsync();
            logger.LogInformation("User {UserId} unliked post {PostId} successfully", userId, postId);
            return ResultResponse<LikeResponse>.Success("Post unliked successfully ");
        }

        //Get Post Likes
        public async Task<ResultResponse<LikeResponse>> GetPostLikesAsync(Guid postId)
        {
            var existPost = await unitOfWork.Repository<Post>().GetByIdAsync(postId);
            if (existPost is null)
            {
                logger.LogWarning("Post {PostId} not found for retrieving likes", postId);
                return ResultResponse<LikeResponse>.Failure("Post not found ");
            }
            var spec = new LikeSpecification(l => l.PostId == postId);
            var likes = await unitOfWork.Repository<Like>().GetAllWithSpec(spec);
            if (!likes.Any())
            {
                logger.LogInformation("No likes found for post {PostId}", postId);
                return ResultResponse<LikeResponse>.Failure("No likes found for this post ");
            }
            var likeDtos = mapper.Map<List<LikeResponse>>(likes);
            logger.LogInformation("Retrieved {Count} likes for post {PostId}", likeDtos.Count, postId);
            return ResultResponse<LikeResponse>.Success(likeDtos, "Likes retrieved successfully ");
        }
    }
}

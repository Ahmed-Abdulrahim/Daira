namespace Daira.Infrastructure.Services.CommentService
{
    public class CommentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommentService> logger, UserManager<AppUser> userManager) : ICommentService
    {
        public async Task<CommentResponse> AddComment(string userId, Guid postId, AddCommentDto addCommentDto)
        {
            var checkpost = await unitOfWork.Repository<Post>().GetByIdAsync(postId);
            if (checkpost is null)
            {
                logger.LogWarning("Post with ID {PostId} not found when adding comment by user {UserId}.", postId, userId);
                return CommentResponse.Failure("Post not found.");
            }
            var checkUser = await userManager.FindByIdAsync(userId);
            if (checkUser is null)
            {
                logger.LogWarning("User with ID {UserId} not found when adding comment to post {PostId}.", userId, postId);
                return CommentResponse.Failure("User not found.");
            }
            var newComment = new Comment
            {
                Content = addCommentDto.Content,
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };
            await unitOfWork.Repository<Comment>().AddAsync(newComment);
            await unitOfWork.CommitAsync();
            var mapComment = mapper.Map<CommentDto>(newComment);
            return CommentResponse.Success(mapComment, "Comment added successfully.");
        }
    }
}

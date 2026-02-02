namespace Daira.Infrastructure.Services.CommentService
{
    public class CommentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommentService> logger, UserManager<AppUser> userManager) : ICommentService
    {
        //AddComment
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

        //DeleteComment
        public async Task<CommentResponse> DeleteComment(string userId, Guid commentId)
        {
            var checkCommnet = await unitOfWork.Repository<Comment>().GetByIdAsync(commentId);
            if (checkCommnet is null)
            {
                logger.LogWarning("Comment with ID {CommentId} not found when deleting by user {UserId}.", commentId, userId);
                return CommentResponse.Failure("Comment not found.");
            }
            if (checkCommnet.UserId != userId)
            {
                logger.LogWarning("User with ID {UserId} attempted to delete comment {CommentId} without permission.", userId, commentId);
                return CommentResponse.Failure("You do not have permission to delete this comment.");
            }
            var mapDeletedComment = mapper.Map<CommentDto>(checkCommnet);
            unitOfWork.Repository<Comment>().Delete(checkCommnet);
            await unitOfWork.CommitAsync();

            return CommentResponse.Success(mapDeletedComment, "Comment deleted successfully.");
        }

        // GetCommentsByPostId
        public async Task<CommentResponse> GetCommentsByPostId(Guid postId)
        {
            var comments = new CommentSpecification(c => c.PostId == postId);
            var commentList = await unitOfWork.Repository<Comment>().GetAllWithSpec(comments);
            if (!commentList.Any())
            {
                logger.LogInformation("No comments found for post with ID {PostId}.", postId);
                return CommentResponse.Failure("No comments found for this post.");
            }
            var mapList = mapper.Map<List<CommentDto>>(commentList);
            return CommentResponse.Success(mapList, "Comments retrieved successfully.");
        }

        //UpdateComment
        public async Task<CommentResponse> UpdateComment(string userId, Guid commentId, AddCommentDto updateCommentDto)
        {
            var checkComment = await unitOfWork.Repository<Comment>().GetByIdAsync(commentId);
            if (checkComment is null)
            {
                logger.LogWarning("Comment with ID {CommentId} not found when updating by user {UserId}.", commentId, userId);
                return CommentResponse.Failure("Comment not found.");
            }
            if (checkComment.UserId != userId)
            {
                logger.LogWarning("User with ID {UserId} attempted to update comment {CommentId} without permission.", userId, commentId);
                return CommentResponse.Failure("You do not have permission to update this comment.");
            }
            checkComment.Content = updateCommentDto.Content;
            checkComment.UpdatedAt = DateTime.UtcNow;
            unitOfWork.Repository<Comment>().Update(checkComment);
            await unitOfWork.CommitAsync();
            var mapComment = mapper.Map<CommentDto>(checkComment);
            return CommentResponse.Success(mapComment, "Comment updated successfully.");
        }
    }
}

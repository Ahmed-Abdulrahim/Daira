namespace Daira.Infrastructure.Services.CommentService
{
    public class CommentService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CommentService> logger, UserManager<AppUser> userManager) : ICommentService
    {
        //AddComment
        public async Task<ResultResponse<CommentResponse>> AddComment(string userId, Guid postId, AddCommentDto addCommentDto)
        {
            var checkpost = await unitOfWork.Repository<Post>().GetByIdAsync(postId);
            if (checkpost is null)
            {
                logger.LogWarning("Post with ID {PostId} not found when adding comment by user {UserId}.", postId, userId);
                return ResultResponse<CommentResponse>.Failure("Post not found.");
            }
            var checkUser = await userManager.FindByIdAsync(userId);
            if (checkUser is null)
            {
                logger.LogWarning("User with ID {UserId} not found when adding comment to post {PostId}.", userId, postId);
                return ResultResponse<CommentResponse>.Failure("User not found.");
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
            var mapComment = mapper.Map<CommentResponse>(newComment);
            return ResultResponse<CommentResponse>.Success(mapComment, "Comment added successfully.");
        }

        //DeleteComment
        public async Task<ResultResponse<CommentResponse>> DeleteComment(string userId, Guid commentId)
        {
            var checkCommnet = await unitOfWork.Repository<Comment>().GetByIdAsync(commentId);
            if (checkCommnet is null)
            {
                logger.LogWarning("Comment with ID {CommentId} not found when deleting by user {UserId}.", commentId, userId);
                return ResultResponse<CommentResponse>.Failure("Comment not found.");
            }
            if (checkCommnet.UserId != userId)
            {
                logger.LogWarning("User with ID {UserId} attempted to delete comment {CommentId} without permission.", userId, commentId);
                return ResultResponse<CommentResponse>.Failure("You do not have permission to delete this comment.");
            }
            var mapDeletedComment = mapper.Map<CommentResponse>(checkCommnet);
            unitOfWork.Repository<Comment>().Delete(checkCommnet);
            await unitOfWork.CommitAsync();

            return ResultResponse<CommentResponse>.Success(mapDeletedComment, "Comment deleted successfully.");
        }

        // GetCommentsByPostId
        public async Task<ResultResponse<CommentResponse>> GetCommentsByPostId(Guid postId)
        {
            var comments = new CommentSpecification(c => c.PostId == postId);
            var commentList = await unitOfWork.Repository<Comment>().GetAllWithSpec(comments);
            if (!commentList.Any())
            {
                logger.LogInformation("No comments found for post with ID {PostId}.", postId);
                return ResultResponse<CommentResponse>.Failure("No comments found for this post.");
            }
            var mapList = mapper.Map<List<CommentResponse>>(commentList);
            return ResultResponse<CommentResponse>.Success(mapList, "Comments retrieved successfully.");
        }

        //UpdateComment
        public async Task<ResultResponse<CommentResponse>> UpdateComment(string userId, Guid commentId, AddCommentDto updateCommentDto)
        {
            var checkComment = await unitOfWork.Repository<Comment>().GetByIdAsync(commentId);
            if (checkComment is null)
            {
                logger.LogWarning("Comment with ID {CommentId} not found when updating by user {UserId}.", commentId, userId);
                return ResultResponse<CommentResponse>.Failure("Comment not found.");
            }
            if (checkComment.UserId != userId)
            {
                logger.LogWarning("User with ID {UserId} attempted to update comment {CommentId} without permission.", userId, commentId);
                return ResultResponse<CommentResponse>.Failure("You do not have permission to update this comment.");
            }
            checkComment.Content = updateCommentDto.Content;
            checkComment.UpdatedAt = DateTime.UtcNow;
            unitOfWork.Repository<Comment>().Update(checkComment);
            await unitOfWork.CommitAsync();
            var mapComment = mapper.Map<CommentResponse>(checkComment);
            return ResultResponse<CommentResponse>.Success(mapComment, "Comment updated successfully.");
        }
    }
}

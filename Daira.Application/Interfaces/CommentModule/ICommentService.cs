namespace Daira.Application.Interfaces.CommentModule
{
    public interface ICommentService
    {
        //Adds a comment to a post
        Task<ResultResponse<CommentResponse>> AddComment(string userId, Guid postId, AddCommentDto addCommentDto);
        //Deletes a comment from a post
        Task<ResultResponse<CommentResponse>> DeleteComment(string userId, Guid commentId);
        //Updates a comment on a post
        Task<ResultResponse<CommentResponse>> UpdateComment(string userId, Guid commentId, AddCommentDto updateCommentDto);
        //Gets comments by post ID
        Task<ResultResponse<CommentResponse>> GetCommentsByPostId(Guid postId);
    }
}

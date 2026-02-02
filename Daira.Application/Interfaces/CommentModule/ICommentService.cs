namespace Daira.Application.Interfaces.CommentModule
{
    public interface ICommentService
    {
        //Adds a comment to a post
        Task<CommentResponse> AddComment(string userId, Guid postId, AddCommentDto addCommentDto);
        //Deletes a comment from a post
        Task<CommentResponse> DeleteComment(string userId, Guid commentId);
        //Updates a comment on a post
        Task<CommentResponse> UpdateComment(string userId, Guid commentId, AddCommentDto updateCommentDto);
        //Gets comments by post ID
        Task<CommentResponse> GetCommentsByPostId(Guid postId);
    }
}

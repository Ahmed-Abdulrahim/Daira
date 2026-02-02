namespace Daira.Application.Interfaces.CommentModule
{
    public interface ICommentService
    {
        Task<CommentResponse> AddComment(string userId, Guid postId, AddCommentDto addCommentDto);
    }
}

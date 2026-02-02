namespace Daira.Application.Interfaces.PostModule
{
    public interface IPostService
    {
        Task<CreatePostResponse> CreatePostAsync(string userId, CreatePostDto createPostDto);
        Task<PostResponse> GetPostByIdAsync(Guid postId);

        Task<UpdateResponse> UpdatePostAsync(Guid postId, string userId, UpdatePostDto updatePostDto);

        Task<PostResponse> DeletePostAsync(string usreId, Guid PostId);

    }
}

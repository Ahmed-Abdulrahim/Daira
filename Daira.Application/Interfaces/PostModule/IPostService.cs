namespace Daira.Application.Interfaces.PostModule
{
    public interface IPostService
    {
        Task<CreatePostResponse> CreatePostAsync(string userId, CreatePostDto createPostDto);
    }
}

namespace Daira.Application.Interfaces.PostModule
{
    public interface IPostService
    {
        // Create Post
        Task<CreatePostResponse> CreatePostAsync(string userId, CreatePostDto createPostDto);
        // Get Post By Id
        Task<PostResponse> GetPostByIdAsync(Guid postId);
        // Update Post
        Task<UpdateResponse> UpdatePostAsync(Guid postId, string userId, UpdatePostDto updatePostDto);
        // Delete Post
        Task<PostResponse> DeletePostAsync(string usreId, Guid PostId);
        // Get All Posts For Specific User
        Task<PostResponse> GetAllPostForSpecificUser(string userId);
        // Get Feed Posts
        Task<PostResponse> GetFeedAsync(string userId, int pageNumber = 1, int pageSize = 10);


    }
}

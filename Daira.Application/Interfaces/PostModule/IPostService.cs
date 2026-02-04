namespace Daira.Application.Interfaces.PostModule
{
    public interface IPostService
    {
        // Create Post
        Task<ResultResponse<PostResponse>> CreatePostAsync(string userId, CreatePostDto createPostDto);
        // Get Post By Id
        Task<ResultResponse<PostResponse>> GetPostByIdAsync(Guid postId);
        // Update Post
        Task<ResultResponse<PostResponse>> UpdatePostAsync(Guid postId, string userId, UpdatePostDto updatePostDto);
        // Delete Post
        Task<ResultResponse<PostResponse>> DeletePostAsync(string usreId, Guid PostId);
        // Get All Posts For Specific User
        Task<ResultResponse<PostResponse>> GetAllPostForSpecificUser(string userId);
        // Get Feed Posts
        Task<ResultResponse<PostResponse>> GetFeedAsync(string userId, int pageNumber = 1, int pageSize = 10);

        //LikePost
        Task<ResultResponse<LikeResponse>> LikePostAsync(string userId, Guid postId);

        //UnLikePost
        Task<ResultResponse<LikeResponse>> UnLikePostAsync(string userId, Guid postId);

        //Get Post Likes
        Task<ResultResponse<LikeResponse>> GetPostLikesAsync(Guid postId);

    }
}

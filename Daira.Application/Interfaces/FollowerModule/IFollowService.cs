namespace Daira.Application.Interfaces.FollowerModule
{
    public interface IFollowService
    {
        //Follow User
        Task<ResultResponse<FollowerResponse>> FollowUserAsync(string followerId, string followeeId);
        //UnFollow User
        Task<ResultResponse<FollowerResponse>> UnFollowUserAsync(string followerId, string followeeId);

        //Get All Followers of a User
        Task<ResultResponse<FollowerResponse>> GetFollowersAsync(string userId);
        //Get All Following of a User
        Task<ResultResponse<FollowerResponse>> GetFollowingAsync(string userId);
    }

}

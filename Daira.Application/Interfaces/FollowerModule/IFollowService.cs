namespace Daira.Application.Interfaces.FollowerModule
{
    public interface IFollowService
    {
        //Follow User
        Task<FollowerResponse> FollowUserAsync(string followerId, string followeeId);
        //UnFollow User
        Task<FollowerResponse> UnFollowUserAsync(string followerId, string followeeId);

        //Get All Followers of a User
        Task<FollowerResponse> GetFollowersAsync(string userId);
        //Get All Following of a User
        Task<FollowerResponse> GetFollowingAsync(string userId);
    }

}

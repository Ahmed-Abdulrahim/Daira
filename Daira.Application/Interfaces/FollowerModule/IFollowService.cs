namespace Daira.Application.Interfaces.FollowerModule
{
    public interface IFollowService
    {
        //Follow User
        Task<FollowerResponse> FollowUserAsync(string followerId, string followeeId);
    }
}

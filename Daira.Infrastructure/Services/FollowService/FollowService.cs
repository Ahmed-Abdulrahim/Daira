namespace Daira.Infrastructure.Services.FollowService
{
    public class FollowService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FollowService> logger, UserManager<AppUser> userManager) : IFollowService
    {
        //Follow User
        public async Task<FollowerResponse> FollowUserAsync(string followerId, string followeeId)
        {
            if (followerId == followeeId)
            {
                logger.LogWarning("FollowUserAsync: User {FollowerId} attempted to follow themselves.", followerId);
                return FollowerResponse.Failure("You cannot follow yourself.");
            }
            var follower = await userManager.FindByIdAsync(followerId);
            if (follower is null)
            {
                logger.LogWarning("FollowUserAsync: Follower with ID {FollowerId} not found.", followerId);
                return FollowerResponse.Failure("Follower not found.");
            }
            var followee = await userManager.FindByIdAsync(followeeId);
            if (followee is null)
            {
                logger.LogWarning("FollowUserAsync: Followee with ID {FolloweeId} not found.", followeeId);
                return FollowerResponse.Failure("Followee not found.");
            }
            var spec = new FollowerSpecification(f => f.FollowerId == followerId && f.FollowingId == followeeId);
            var followExist = await unitOfWork.Repository<Follower>().GetByIdSpecTracked(spec);
            if (followExist is not null)
            {
                logger.LogInformation("FollowUserAsync: User {FollowerId} already follows {FolloweeId}.", followerId, followeeId);
                return FollowerResponse.Failure("You are already following this user.");
            }
            var follow = new Follower
            {
                FollowerId = followerId,
                FollowingId = followeeId,
                CreatedAt = DateTime.UtcNow
            };
            await unitOfWork.Repository<Follower>().AddAsync(follow);
            await unitOfWork.CommitAsync();
            logger.LogInformation("FollowUserAsync: User {FollowerId} successfully followed {FolloweeId}.", followerId, followeeId);
            var followDto = mapper.Map<FollowerDto>(follow);
            return FollowerResponse.Success(followDto, "Successfully followed the user.");

        }
    }
}

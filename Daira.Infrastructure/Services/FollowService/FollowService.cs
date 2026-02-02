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

        //UnFollow User
        public async Task<FollowerResponse> UnFollowUserAsync(string followerId, string followeeId)
        {
            if (followeeId == followerId)
            {
                logger.LogWarning("User {FollowerId} attempted to unfollow themselves.", followerId);
                return FollowerResponse.Failure("You cannot unfollow yourself.");
            }
            var follower = await userManager.FindByIdAsync(followerId);
            if (follower is null)
            {
                logger.LogWarning(" Follower with ID {FollowerId} not found.", followerId);
                return FollowerResponse.Failure("Follower not found.");
            }
            var followee = await userManager.FindByIdAsync(followeeId);
            if (followee is null)
            {
                logger.LogWarning(" Followee with ID {FolloweeId} not found.", followeeId);
                return FollowerResponse.Failure("Followee not found.");
            }
            var spec = new FollowerSpecification(f => f.FollowerId == followerId && f.FollowingId == followeeId);
            var followExist = await unitOfWork.Repository<Follower>().GetByIdSpecTracked(spec);
            if (followExist is null)
            {
                logger.LogInformation(" User {FollowerId} does not follow {FolloweeId}.", followerId, followeeId);
                return FollowerResponse.Failure("You are not following this user.");
            }
            unitOfWork.Repository<Follower>().Delete(followExist);
            await unitOfWork.CommitAsync();
            logger.LogInformation(" User {FollowerId} successfully unfollowed {FolloweeId}.", followerId, followeeId);
            return FollowerResponse.Success("Successfully unfollowed the user.");

        }

        //Get All Followers of a User
        public async Task<FollowerResponse> GetFollowersAsync(string userId)
        {
            var existingUser = await userManager.FindByIdAsync(userId);
            if (existingUser is null)
            {
                logger.LogWarning("GetFollowersAsync: User with ID {UserId} not found.", userId);
                return FollowerResponse.Failure("User not found.");
            }
            var spec = new FollowerSpecification(f => f.FollowingId == userId);
            var followers = await unitOfWork.Repository<Follower>().GetAllWithSpec(spec);
            if (!followers.Any())
            {
                logger.LogInformation("GetFollowersAsync: User {UserId} has no followers.", userId);
                return FollowerResponse.Success(new List<FollowerDto>(), "User has no followers.");
            }
            var followerDtos = mapper.Map<List<FollowerDto>>(followers);
            logger.LogInformation("GetFollowersAsync: Retrieved {Count} followers for user {UserId}.", followerDtos.Count, userId);
            return FollowerResponse.Success(followerDtos, "Followers retrieved successfully.");
        }

        //Get All Following of a User
        public async Task<FollowerResponse> GetFollowingAsync(string userId)
        {
            var existingUser = await userManager.FindByIdAsync(userId);
            if (existingUser is null)
            {
                logger.LogWarning("GetFollowingAsync: User with ID {UserId} not found.", userId);
                return FollowerResponse.Failure("User not found.");
            }
            var spec = new FollowerSpecification(f => f.FollowerId == userId);
            var followers = await unitOfWork.Repository<Follower>().GetAllWithSpec(spec);
            if (!followers.Any())
            {
                logger.LogInformation("GetFollowingAsync: User {UserId} has no followers.", userId);
                return FollowerResponse.Success(new List<FollowerDto>(), "User has no Following.");
            }
            var followerDtos = mapper.Map<List<FollowerDto>>(followers);
            logger.LogInformation("GetFollowingAsync: Retrieved {Count} followers for user {UserId}.", followerDtos.Count, userId);
            return FollowerResponse.Success(followerDtos, "Following retrieved successfully.");
        }
    }
}
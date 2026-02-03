namespace Daira.Infrastructure.Services.FriendShipService
{
    public class FriendShipService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<FriendShipService> logger, UserManager<AppUser> userManager) : IFriendShipService
    {
        //Accept FriendShip
        public async Task<FriendshipResponse> AcceptFriendRequest(Guid id)
        {
            var existFriendShip = await unitOfWork.Repository<Friendship>().GetByIdAsync(id);
            if (existFriendShip is null)
            {
                logger.LogWarning("AcceptFriendAsync:FriendShip is nopt Found");
                return FriendshipResponse.Failure("FriendShip is not found");
            }
            if (existFriendShip.Status == RequestStatus.Declined || existFriendShip.Status == RequestStatus.Accepted || existFriendShip.Status == RequestStatus.Blocked)
            {
                logger.LogWarning("AcceptFriendShipAsync: cant Accept This friendShip due To Request Status");
                return FriendshipResponse.Failure("Cannot Accept This FriendShip due To Request Status ");
            }
            existFriendShip.Status = RequestStatus.Accepted;
            existFriendShip.UpdatedAt = DateTime.UtcNow;
            unitOfWork.Repository<Friendship>().Update(existFriendShip);
            await unitOfWork.CommitAsync();
            return FriendshipResponse.Success("Successfully Accept Request the user");
        }
        // Send Friend Request
        public async Task<FriendshipResponse> SendFriendRequestAsync(string requestId, string addressId)
        {
            if (requestId == addressId)
            {
                logger.LogWarning("FriendShipUserAsync: User {userId} attempted to sendRequest themselves.", requestId);
                return FriendshipResponse.Failure("You cannot Send Request to yourself.");
            }
            var existAddressId = await userManager.FindByIdAsync(addressId);
            if (existAddressId is null)
            {
                logger.LogWarning("FriendShipUserAsync: addressId with ID {addressId} not found.", addressId);
                return FriendshipResponse.Failure("addressId not found.");
            }
            var spec = new FriendshipSpecification(f => f.RequesterId == requestId && f.AddresseeId == addressId);
            var existFriendShip = await unitOfWork.Repository<Friendship>().GetByIdSpecTracked(spec);
            if (existFriendShip is not null)
            {
                logger.LogWarning("FriendShipUserAsync: FriendShip is Already Existed");
                return FriendshipResponse.Failure("FriendShip is Already Existed");
            }
            var friendShip = new Friendship
            {
                RequesterId = requestId,
                AddresseeId = addressId,
                CreatedAt = DateTime.Now,
                Status = RequestStatus.Pending
            };
            await unitOfWork.Repository<Friendship>().AddAsync(friendShip);
            await unitOfWork.CommitAsync();
            var mapfriendShip = mapper.Map<FriendshipDto>(friendShip);
            return FriendshipResponse.Success(mapfriendShip, "Successfully Send Request the user.");
        }

        //Decline Friend Request
        public async Task<FriendshipResponse> DeclineFriendRequest(Guid id)
        {
            var existFriendShip = await unitOfWork.Repository<Friendship>().GetByIdAsync(id);
            if (existFriendShip is null)
            {
                logger.LogWarning("Decline FrinedShip Async :FriendShip is not Found");
                return FriendshipResponse.Failure("FriendShip is not found");
            }
            if (existFriendShip.Status == RequestStatus.Accepted || existFriendShip.Status == RequestStatus.Blocked)
            {
                logger.LogWarning("Decline FrinedShip Async: cant Decline This friendShip due To Request Status");
                return FriendshipResponse.Failure("Cannot Decline This FriendShip due To Request Status ");
            }
            existFriendShip.Status = RequestStatus.Declined;
            existFriendShip.UpdatedAt = DateTime.UtcNow;
            unitOfWork.Repository<Friendship>().Update(existFriendShip);

            await unitOfWork.CommitAsync();
            return FriendshipResponse.Success("Successfully Decline Request the user");
        }

        //Get All Friend Requests
        public async Task<FriendshipResponse> GetAllFriendRequests(string userId)
        {
            var existUser = await userManager.FindByIdAsync(userId);
            if (existUser is null)
            {
                logger.LogWarning("GetAllFriendRequest: {userId} Not Found ", userId);
                return FriendshipResponse.Failure("User Not Found");
            }
            var spec = new FriendshipSpecification(f => f.RequesterId == userId && f.Status == RequestStatus.Pending);
            var listFriendships = await unitOfWork.Repository<Friendship>().GetAllWithSpec(spec);
            if (!listFriendships.Any())
            {
                logger.LogWarning("GetAllFriendShip: user has no FriendShip");
                return FriendshipResponse.Success("NoFriendRequest Founded");
            }
            var mapListFriendShip = mapper.Map<List<FriendshipDto>>(listFriendships);
            return FriendshipResponse.Success(mapListFriendShip);
        }

        //Get All Friends
        public async Task<FriendshipResponse> GetAllFriends(string userId)
        {
            var existUser = await userManager.FindByIdAsync(userId);
            if (existUser is null)
            {
                logger.LogWarning("GetAllFriends: {userId} Not Found ", userId);
                return FriendshipResponse.Failure("User Not Found");
            }
            var spec = new FriendshipSpecification(f => f.RequesterId == userId && f.Status == RequestStatus.Accepted);
            var listFriendships = await unitOfWork.Repository<Friendship>().GetAllWithSpec(spec);
            if (!listFriendships.Any())
            {
                logger.LogWarning("GetAllFriends: user has no Friends");
                return FriendshipResponse.Success("NoFriend Founded");
            }
            var mapListFriendShip = mapper.Map<List<FriendshipDto>>(listFriendships);
            return FriendshipResponse.Success(mapListFriendShip);
        }

        //UnFriend
        public async Task<FriendshipResponse> UnFriend(Guid Id)
        {
            var existingFriendShip = await unitOfWork.Repository<Friendship>().GetByIdAsync(Id);
            if (existingFriendShip is null)
            {
                logger.LogWarning("UnFriend: {FriendShipId} Not Found ", Id);
                return FriendshipResponse.Failure("FriendShipId Not Found");
            }
            if (existingFriendShip.Status == RequestStatus.Pending || existingFriendShip.Status == RequestStatus.Declined || existingFriendShip.Status == RequestStatus.Blocked)
            {
                logger.LogWarning("UnFriend:cant delete FriendShip With {Status} Not Found ", existingFriendShip.Status);
                return FriendshipResponse.Failure("cannot delete FriendShip");
            }
            unitOfWork.Repository<Friendship>().Delete(existingFriendShip);
            await unitOfWork.CommitAsync();
            return FriendshipResponse.Success("Successfully UnFriend");
        }
    }
}

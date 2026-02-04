namespace Daira.Application.Interfaces.FriendshipModule
{
    public interface IFriendShipService
    {
        //Send Friendship Request 
        Task<ResultResponse<FriendshipResponse>> SendFriendRequestAsync(string requestId, string addressId);

        //Accept Friend request
        Task<ResultResponse<FriendshipResponse>> AcceptFriendRequest(Guid id);
        //Decline Friend request
        Task<ResultResponse<FriendshipResponse>> DeclineFriendRequest(Guid id);

        //Get All Friend Request
        Task<ResultResponse<FriendshipResponse>> GetAllFriendRequests(string userId);
        //Get All Friends
        Task<ResultResponse<FriendshipResponse>> GetAllFriends(string userId);

        //Delete FrindShip (UnFriend)
        Task<ResultResponse<FriendshipResponse>> UnFriend(Guid Id);
    }
}

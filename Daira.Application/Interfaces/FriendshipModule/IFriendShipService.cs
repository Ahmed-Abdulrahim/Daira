namespace Daira.Application.Interfaces.FriendshipModule
{
    public interface IFriendShipService
    {
        //Send Friendship Request 
        Task<FriendshipResponse> SendFriendRequestAsync(string requestId, string addressId);

        //Accept Friend request
        Task<FriendshipResponse> AcceptFriendRequest(Guid id);
        //Decline Friend request
        Task<FriendshipResponse> DeclineFriendRequest(Guid id);

        //Get All Friend Request
        Task<FriendshipResponse> GetAllFriendRequests(string userId);
        //Get All Friends
        Task<FriendshipResponse> GetAllFriends(string userId);

        //Delete FrindShip (UnFriend)
        Task<FriendshipResponse> UnFriend(Guid Id);
    }
}

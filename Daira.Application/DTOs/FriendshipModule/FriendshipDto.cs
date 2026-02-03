namespace Daira.Application.DTOs.FriendshipModule
{
    public class FriendshipDto
    {
        public Guid Id { get; set; }
        public string RequesterId { get; set; }
        public string AddresseeId { get; set; }
        public RequestStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}

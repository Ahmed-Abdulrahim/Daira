namespace Daira.Domain.Models
{
    public enum RequestStatus
    {
        Pending,
        Accepted,
        Declined,
        Blocked,
    }
    public class Friendship : BaseEntity
    {
        public string RequesterId { get; set; }
        public string AddresseeId { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;

        //Navigation Properties
        public AppUser Requester { get; set; }
        public AppUser Addressee { get; set; }
    }
}

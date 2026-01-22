namespace Daira.Domain.Models
{
    public class Follower
    {
        public Guid Id { get; set; }
        public string FollowerId { get; set; }
        public string FollowingId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //Navigation properties
        public AppUser FollowerUser { get; set; }
        public AppUser FollowingUser { get; set; }
    }
}

namespace Daira.Domain.Models
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //Navigation properties
        public Post Post { get; set; }
        public AppUser AppUser { get; set; }
    }
}

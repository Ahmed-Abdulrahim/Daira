namespace Daira.Domain.Models
{
    public class Comment : BaseEntity
    {

        public Guid PostId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        //Navigation properties
        public Post Post { get; set; }
        public AppUser AppUser { get; set; }
    }
}

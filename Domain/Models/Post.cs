namespace Daira.Domain.Models
{
    public class Post
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public int LikesCount { get; set; } = 0;
        public int CommentsCount { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }


        // Navigation property
        public AppUser User { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }


    }
}

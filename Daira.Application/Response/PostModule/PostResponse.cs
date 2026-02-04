namespace Daira.Application.Response.PostModule
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? ImageUrl { get; set; }
        public AuthorResponse Author { get; set; }
        public int LikesCount { get; set; }
        public int CommentsCount { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}

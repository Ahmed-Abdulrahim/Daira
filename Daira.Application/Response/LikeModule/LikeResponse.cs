namespace Daira.Application.Response.LikeModule
{
    public class LikeResponse
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string userId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

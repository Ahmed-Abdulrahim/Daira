namespace Daira.Application.DTOs.LikeModule
{
    public class LikeDto
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public string userId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

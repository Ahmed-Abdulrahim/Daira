namespace Daira.Application.Response.ParticipantsMosule
{
    public class ParticipantsResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? PictureUrl { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}

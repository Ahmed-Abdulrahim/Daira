namespace Daira.Application.DTOs.ConversationModule
{
    public class ParticipantDto
    {
        public string UserId { get; set; } = string.Empty;
        public string? DisplayName { get; set; }
        public string? PictureUrl { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}

namespace Daira.Domain.Models
{
    public enum ConversationType
    {
        Direct,
        Group,
    }
    public class Conversation
    {
        public Guid Id { get; set; }
        public ConversationType Type { get; set; } = ConversationType.Direct;
        public string? Name { get; set; }
        public string CreatedById { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        //Navigation Properties
        public AppUser CreatedBy { get; set; }
        public List<ConversationParticipant> Participants { get; set; }
        public List<Message> Messages { get; set; }
    }
}

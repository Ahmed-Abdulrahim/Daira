namespace Daira.Domain.Models
{
    public class ConversationParticipant : BaseEntity
    {

        public Guid ConversationId { get; set; }
        public string UserId { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.Now;

        //Navigation Properties
        public AppUser AppUser { get; set; }
        public Conversation Conversation { get; set; }
    }
}

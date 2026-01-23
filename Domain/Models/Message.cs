namespace Daira.Domain.Models
{
    public class Message : BaseEntity
    {
        public Guid ConversationId { get; set; }
        public string SenderId { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        //Navigation Properties
        public AppUser Sender { get; set; }
        public Conversation Conversation { get; set; }
    }
}

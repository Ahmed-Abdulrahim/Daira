namespace Daira.Domain.Models.AuthModel
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DisplayName { get; set; }
        public string? Bio { get; set; }
        public string? PictureUrl { get; set; }
        public bool IsVerified { get; set; } = false;
        public bool IsPrivate { get; set; } = false;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? FullName => $"{FirstName} {LastName}".Trim();
        //Navigation Properties
        public List<Post> Posts { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public ICollection<Notification> ReceivedNotifications { get; set; }
        public ICollection<Notification> TriggeredNotifications { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public List<Message> Messages { get; set; }
        public List<Follower> Following { get; set; }
        public List<Follower> Follower { get; set; }
        public List<Friendship> SentFriendRequests { get; set; }
        public List<Friendship> ReceivedFriendRequests { get; set; }
        public List<ConversationParticipant> ConversationParticipants { get; set; }

    }
}


namespace Daira.Domain.Models.AuthModel
{
    public class RefreshToken : BaseEntity
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime? RevokedAt { get; set; }
        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        //Navigation Properties
        public AppUser AppUser { get; set; }
        public void Revoke()
        {
            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
        }
    }
}

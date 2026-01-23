namespace Daira.Domain.Models.AuthModel
{
    public class RefreshToken : BaseEntity
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //Navigation Properties
        public AppUser AppUser { get; set; }
    }
}

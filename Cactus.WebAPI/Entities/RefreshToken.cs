namespace Cactus.WebAPI.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }

        public string Token { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool RememberMe { get; set; }
        public DateTime ExpiresAt { get; set; }

        public DateTime? RevokedAt { get; set; }

        public bool IsRevoked { get; set; }

        public string? CreatedByIp { get; set; }
        public string? CreatedByUserAgent { get; set; }
        public string? RevokedByIp { get; set; }
        public string? RevokedByUserAgent { get; set; }

        public string? ReplacedByToken { get; set; }
        

        public string UserId { get; set; }
        public AppUser User { get; set; }

        public bool IsActive => !IsRevoked && DateTime.UtcNow < ExpiresAt;
    }
}

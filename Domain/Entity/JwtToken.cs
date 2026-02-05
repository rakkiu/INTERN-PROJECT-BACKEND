
namespace Domain.Entity
{
    /// <summary>
    /// Refresh Token entity for JWT authentication
    /// </summary>
    public class JwtToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = null!;
        public string TokenType { get; set; } = "RefreshToken";
        public DateTime ExpiresAt { get; set; }
        public bool IsRevoked { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        // Foreign Keys
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}

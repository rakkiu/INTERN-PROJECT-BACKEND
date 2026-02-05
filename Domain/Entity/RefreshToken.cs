
namespace Domain.Entity
{
    /// <summary>
    /// Refresh Token entity for JWT authentication
    /// </summary>
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Token { get; set; } = null!;
        public bool IsRevoked { get; set; } = false;
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }

        // Foreign Keys
        public User User { get; set; } = null!;
    }
}

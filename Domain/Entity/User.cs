namespace Domain.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }

        // Removed: public Role Role { get; set; } = null!; 

        public ICollection<JwtToken> JwtTokens { get; set; } = new List<JwtToken>();
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}

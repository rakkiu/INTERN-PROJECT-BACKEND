namespace Domain.Entity
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? FullName { get; set; }
        public bool IsActive { get; set; } = true;

        public Guid Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; } // Chỉ lưu hash

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;

        // Navigation Properties
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public ICollection<WorkTask> AssignedTasks { get; set; } = new List<WorkTask>();
        public ICollection<WorkTask> CreatedTasks { get; set; } = new List<WorkTask>();
        public ICollection<Worklog> Worklogs { get; set; } = new List<Worklog>();
    }
}

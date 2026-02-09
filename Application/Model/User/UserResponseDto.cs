namespace Application.Model.User
{
    /// <summary>
    /// Response DTO for User (Admin view).
    /// </summary>
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }
        public bool IsActive { get; set; }
        public string RoleName { get; set; } = null!;
        public Guid RoleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

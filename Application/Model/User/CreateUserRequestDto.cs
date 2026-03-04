namespace Application.Model.User
{
    /// <summary>
    /// Request DTO for creating a new User (Admin only).
    /// Password will be auto-generated and sent to user email.
    /// </summary>
    public class CreateUserRequestDto
    {
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }
        public Guid RoleId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

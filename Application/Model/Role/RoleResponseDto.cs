namespace Application.Model.Role
{
    /// <summary>
    /// Response DTO for Role.
    /// </summary>
    public class RoleResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public int UserCount { get; set; }
    }
}

namespace Application.Model.User
{
    /// <summary>
    /// Request DTO for updating an existing User (Admin only).
    /// </summary>
    public class UpdateUserRequestDto
    {
        public string? FullName { get; set; }
        public Guid? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}

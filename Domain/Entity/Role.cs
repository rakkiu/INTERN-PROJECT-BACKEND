namespace Domain.Entity
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        public ICollection<RolePrivilege> RolePrivileges { get; set; } = new List<RolePrivilege>(); // THÊM DÒNG NÀY
    }
}

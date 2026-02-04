namespace Domain.Entity
{
    public class Privilege
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!; // e.g., "user.create", "product.delete"
        public string? Description { get; set; }
        public string? Category { get; set; } // e.g., "User Management", "Product Management"

        public ICollection<RolePrivilege> RolePrivileges { get; set; } = new List<RolePrivilege>();
    }
}
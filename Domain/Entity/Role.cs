namespace Domain.Entity
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!; // ADMIN, LEADER, MEMBER
        public string? Description { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<RolePrivilege> RolePrivileges { get; set; } = new List<RolePrivilege>();
    }
    
    /// <summary>
    /// Role names constants for the system
    /// </summary>
    public static class RoleNames
    {
        public const string ADMIN = "ADMIN";
        public const string LEADER = "LEADER";
        public const string MEMBER = "MEMBER";
    }
}

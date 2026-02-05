namespace Domain.Entity
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!; // ADMIN, LEADER, MEMBER
        public DateTime CreatedAt { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
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

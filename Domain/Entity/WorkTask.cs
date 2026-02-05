namespace Domain.Entity
{
    /// <summary>
    /// WorkTask entity for task management
    /// </summary>
    public class WorkTask
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.MEDIUM;
        public TaskStatus Status { get; set; } = TaskStatus.TODO;
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign Keys
        public Guid? AssigneeId { get; set; }
        public User? Assignee { get; set; }
        
        public Guid CreatedById { get; set; }
        public User CreatedBy { get; set; } = null!;

        // Navigation Properties
        public ICollection<Worklog> Worklogs { get; set; } = new List<Worklog>();
    }

    public enum TaskPriority
    {
        LOW = 0,
        MEDIUM = 1,
        HIGH = 2
    }

    public enum TaskStatus
    {
        TODO = 0,
        IN_PROGRESS = 1,
        DONE = 2,
        BLOCKED = 3,
        OVERDUE = 4
    }
}

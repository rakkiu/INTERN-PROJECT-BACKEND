namespace Domain.Entity
{
    /// <summary>
    /// Worklog entity for tracking work hours on tasks
    /// </summary>
    public class Worklog
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursSpent { get; set; }
        public string? Note { get; set; }
        
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Foreign Keys
        public Guid TaskId { get; set; }
        public Task Task { get; set; } = null!;
        
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
    }
}

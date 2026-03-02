namespace Application.Model.Worklog
{
    public class WorklogResponseDto
    {
        public Guid Id { get; set; }
        public Guid TaskId { get; set; }
        public string TaskTitle { get; set; } = null!;
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursSpent { get; set; }
        public string? Note { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

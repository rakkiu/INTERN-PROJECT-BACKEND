namespace Application.Model.Worklog
{
    public class CreateWorklogRequestDto
    {
        public Guid TaskId { get; set; }
        public DateTime Date { get; set; }
        public decimal HoursSpent { get; set; }
        public string? Note { get; set; }
    }
}

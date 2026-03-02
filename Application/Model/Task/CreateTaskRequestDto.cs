namespace Application.Model.Task
{
    public class CreateTaskRequestDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int Priority { get; set; } = 1; // MEDIUM
    }
}

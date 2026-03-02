namespace Application.Model.Task
{
    public class UpdateTaskRequestDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public DateTime? Deadline { get; set; }
        public int? Priority { get; set; }
        public int? Status { get; set; }
    }
}

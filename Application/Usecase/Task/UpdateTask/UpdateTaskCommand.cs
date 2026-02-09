using MediatR;
using Application.Model.Task;

namespace Application.Usecase.Task.UpdateTask
{
    public record UpdateTaskCommand(Guid TaskId, string? Title, string? Description, DateTime? Deadline, int? Priority, int? Status) 
        : IRequest<TaskDto>;
}

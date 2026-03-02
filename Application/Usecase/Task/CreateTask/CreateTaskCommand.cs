using MediatR;
using Application.Model.Task;

namespace Application.Usecase.Task.CreateTask
{
    public record CreateTaskCommand(string Title, string? Description, DateTime? Deadline, int Priority, Guid CreatedById) 
        : IRequest<TaskDto>;
}

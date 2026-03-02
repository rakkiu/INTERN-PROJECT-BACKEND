using MediatR;
using Application.Model.Task;

namespace Application.Usecase.Task.AssignTask
{
    public record AssignTaskCommand(Guid TaskId, Guid AssigneeId) 
        : IRequest<TaskDto>;
}

using MediatR;
using Application.Model.Task;

namespace Application.Usecase.Task.GetTasks
{
    public record GetTasksQuery() 
        : IRequest<IEnumerable<TaskDto>>;
}

using MediatR;

namespace Application.Usecase.Task.DeleteTask
{
    public record DeleteTaskCommand(Guid TaskId) 
        : IRequest<bool>;
}

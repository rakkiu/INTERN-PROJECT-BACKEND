using Application.Model.Worklog;
using MediatR;

namespace Application.Usecase.Worklog.Create
{
    public record CreateWorklogCommand(
        Guid UserId,
        Guid TaskId,
        DateTime Date,
        decimal HoursSpent,
        string? Note
    ) : IRequest<WorklogResponseDto>;
}

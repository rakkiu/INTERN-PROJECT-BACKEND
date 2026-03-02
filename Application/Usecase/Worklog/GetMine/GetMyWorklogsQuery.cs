using Application.Model.Worklog;
using MediatR;

namespace Application.Usecase.Worklog.GetMine
{
    public record GetMyWorklogsQuery(Guid UserId) : IRequest<IEnumerable<WorklogResponseDto>>;
}

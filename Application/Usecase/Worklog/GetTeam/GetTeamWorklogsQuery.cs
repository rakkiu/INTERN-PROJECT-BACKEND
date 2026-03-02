using Application.Model.Worklog;
using MediatR;

namespace Application.Usecase.Worklog.GetTeam
{
    public record GetTeamWorklogsQuery() : IRequest<IEnumerable<WorklogResponseDto>>;
}

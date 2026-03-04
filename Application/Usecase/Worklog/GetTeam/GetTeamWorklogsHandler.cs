using Application.Model.Worklog;
using Domain.Interface;
using MediatR;

namespace Application.Usecase.Worklog.GetTeam
{
    public class GetTeamWorklogsHandler : IRequestHandler<GetTeamWorklogsQuery, IEnumerable<WorklogResponseDto>>
    {
        private readonly IWorklogRepository _worklogRepository;

        public GetTeamWorklogsHandler(IWorklogRepository worklogRepository)
        {
            _worklogRepository = worklogRepository;
        }

        public async System.Threading.Tasks.Task<IEnumerable<WorklogResponseDto>> Handle(GetTeamWorklogsQuery request, CancellationToken cancellationToken)
        {
            var worklogs = await _worklogRepository.GetAllAsync();

            return worklogs.Select(w => new WorklogResponseDto
            {
                Id = w.Id,
                TaskId = w.TaskId,
                TaskTitle = w.Task?.Title ?? string.Empty,
                UserId = w.UserId,
                UserName = w.User?.FullName,
                Date = w.Date,
                HoursSpent = w.HoursSpent,
                Note = w.Note,
                CreatedAt = w.CreatedAt
            }).ToList();
        }
    }
}

using Application.Model.Worklog;
using Domain.Interface;
using MediatR;

namespace Application.Usecase.Worklog.GetMine
{
    public class GetMyWorklogsHandler : IRequestHandler<GetMyWorklogsQuery, IEnumerable<WorklogResponseDto>>
    {
        private readonly IWorklogRepository _worklogRepository;

        public GetMyWorklogsHandler(IWorklogRepository worklogRepository)
        {
            _worklogRepository = worklogRepository;
        }

        public async System.Threading.Tasks.Task<IEnumerable<WorklogResponseDto>> Handle(GetMyWorklogsQuery request, CancellationToken cancellationToken)
        {
            var worklogs = await _worklogRepository.GetByUserIdAsync(request.UserId);

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

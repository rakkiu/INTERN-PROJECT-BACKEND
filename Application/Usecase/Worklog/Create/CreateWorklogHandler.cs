using Application.Model.Worklog;
using Domain.Interface;
using MediatR;

namespace Application.Usecase.Worklog.Create
{
    public class CreateWorklogHandler : IRequestHandler<CreateWorklogCommand, WorklogResponseDto>
    {
        private readonly IWorklogRepository _worklogRepository;
        private readonly ITaskRepository _taskRepository;

        public CreateWorklogHandler(IWorklogRepository worklogRepository, ITaskRepository taskRepository)
        {
            _worklogRepository = worklogRepository;
            _taskRepository = taskRepository;
        }

        public async System.Threading.Tasks.Task<WorklogResponseDto> Handle(CreateWorklogCommand request, CancellationToken cancellationToken)
        {
            var logDate = request.Date.Date;

            // 1. Cannot log for future dates
            if (logDate > DateTime.UtcNow.Date)
                throw new InvalidOperationException("Cannot log work for a future date.");

            // 2. hoursSpent must be 0 < h ≤ 8
            if (request.HoursSpent <= 0 || request.HoursSpent > 8)
                throw new ArgumentException("Hours spent must be greater than 0 and at most 8.");

            // 3. Task must exist
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new KeyNotFoundException($"Task '{request.TaskId}' not found.");

            // 4. Task must be assigned to this user
            if (task.AssigneeId != request.UserId)
                throw new InvalidOperationException("You can only log work for tasks assigned to you.");

            // 5. Cannot log for DONE tasks
            if (task.Status == Domain.Entity.TaskStatus.DONE)
                throw new InvalidOperationException("Cannot log work for a completed (DONE) task.");

            // 6. One worklog per user/task/day
            var alreadyExists = await _worklogRepository.ExistsAsync(request.UserId, request.TaskId, logDate);
            if (alreadyExists)
                throw new InvalidOperationException("A worklog for this task on the specified date already exists.");

            // 7. Total hours in the day must not exceed 8
            var totalHoursToday = await _worklogRepository.GetTotalHoursByUserAndDateAsync(request.UserId, logDate);
            if (totalHoursToday + request.HoursSpent > 8)
                throw new InvalidOperationException($"Adding {request.HoursSpent}h would exceed the daily limit of 8h. Already logged: {totalHoursToday}h.");

            // 8. Create worklog
            var worklog = new Domain.Entity.Worklog
            {
                Id = Guid.NewGuid(),
                TaskId = request.TaskId,
                UserId = request.UserId,
                Date = logDate,
                HoursSpent = request.HoursSpent,
                Note = request.Note
            };

            var created = await _worklogRepository.CreateAsync(worklog);

            return new WorklogResponseDto
            {
                Id = created.Id,
                TaskId = created.TaskId,
                TaskTitle = task.Title,
                UserId = created.UserId,
                UserName = task.Assignee?.FullName,
                Date = created.Date,
                HoursSpent = created.HoursSpent,
                Note = created.Note,
                CreatedAt = created.CreatedAt
            };
        }
    }
}

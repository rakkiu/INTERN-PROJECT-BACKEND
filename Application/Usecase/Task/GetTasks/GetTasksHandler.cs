using Application.Model.Task;
using Domain.Interface;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Task.GetTasks
{
    public class GetTasksHandler : IRequestHandler<GetTasksQuery, IEnumerable<TaskDto>>
    {
        private readonly ITaskRepository _taskRepository;

        public GetTasksHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
        {
            var tasks = await _taskRepository.GetAllAsync();

            return tasks.Select(t => MapToDto(t)).ToList();
        }

        private TaskDto MapToDto(Domain.Entity.WorkTask task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Deadline = task.Deadline,
                Priority = (int)task.Priority,
                Status = (int)task.Status,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                AssigneeId = task.AssigneeId,
                AssigneeName = task.Assignee?.FullName,
                CreatedById = task.CreatedById,
                CreatedByName = task.CreatedBy?.FullName ?? string.Empty,
                WorklogCount = task.Worklogs?.Count ?? 0
            };
        }
    }
}

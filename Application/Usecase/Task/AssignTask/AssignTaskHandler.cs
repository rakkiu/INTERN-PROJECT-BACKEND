using Application.Model.Task;
using Domain.Interface;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Usecase.Task.AssignTask
{
    public class AssignTaskHandler : IRequestHandler<AssignTaskCommand, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public AssignTaskHandler(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<TaskDto> Handle(AssignTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new InvalidOperationException("Task not found");

            var assignee = await _userRepository.GetByIdAsync(request.AssigneeId);
            if (assignee == null)
                throw new InvalidOperationException("Assignee not found");

            task.AssigneeId = request.AssigneeId;
            task.UpdatedAt = DateTime.UtcNow;

            var updatedTask = await _taskRepository.UpdateAsync(task);

            return MapToDto(updatedTask);
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

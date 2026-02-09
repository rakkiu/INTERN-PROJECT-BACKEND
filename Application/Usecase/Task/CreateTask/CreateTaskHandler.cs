using Application.Model.Task;
using Domain.Entity;
using Domain.Interface;
using Domain.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Usecase.Task.CreateTask
{
    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;

        public CreateTaskHandler(ITaskRepository taskRepository, IUserRepository userRepository)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
        }

        public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            var createdBy = await _userRepository.GetByIdAsync(request.CreatedById);
            if (createdBy == null)
                throw new InvalidOperationException("User not found");

            var workTask = new WorkTask
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Deadline = request.Deadline,
                Priority = (TaskPriority)request.Priority,
                Status = Domain.Entity.TaskStatus.TODO,
                CreatedById = request.CreatedById,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var createdTask = await _taskRepository.CreateAsync(workTask);

            return MapToDto(createdTask, createdBy.FullName);
        }

        private TaskDto MapToDto(WorkTask task, string createdByName)
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
                CreatedByName = createdByName,
                WorklogCount = task.Worklogs?.Count ?? 0
            };
        }
    }
}

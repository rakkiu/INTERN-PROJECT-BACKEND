using Application.Model.Task;
using Domain.Interface;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Task.UpdateTask
{
    public class UpdateTaskHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
    {
        private readonly ITaskRepository _taskRepository;

        public UpdateTaskHandler(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken cancellationToken)
        {
            var task = await _taskRepository.GetByIdAsync(request.TaskId);
            if (task == null)
                throw new InvalidOperationException("Task not found");

            if (!string.IsNullOrEmpty(request.Title))
                task.Title = request.Title;

            if (request.Description != null)
                task.Description = request.Description;

            if (request.Deadline != null)
                task.Deadline = request.Deadline;

            if (request.Priority.HasValue)
                task.Priority = (Domain.Entity.TaskPriority)request.Priority;

            if (request.Status.HasValue)
                task.Status = (Domain.Entity.TaskStatus)request.Status;

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

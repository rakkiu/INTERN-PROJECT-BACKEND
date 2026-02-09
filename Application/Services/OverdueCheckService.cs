using Domain.Entity;
using Domain.Interface;
using Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public interface IOverdueCheckService
    {
        Task CheckAndUpdateOverdueTasksAsync(CancellationToken cancellationToken = default);
    }

    public class OverdueCheckService : IOverdueCheckService
    {
        private readonly ITaskRepository _taskRepository;

        public OverdueCheckService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task CheckAndUpdateOverdueTasksAsync(CancellationToken cancellationToken = default)
        {
            var allTasks = await _taskRepository.GetAllAsync();
            var now = DateTime.UtcNow;

            var overdueTasks = allTasks.Where(t =>
                t.Deadline.HasValue &&
                t.Deadline < now &&
                t.Status != Domain.Entity.TaskStatus.DONE &&
                t.Status != Domain.Entity.TaskStatus.OVERDUE
            ).ToList();

            foreach (var task in overdueTasks)
            {
                task.Status = Domain.Entity.TaskStatus.OVERDUE;
                task.UpdatedAt = DateTime.UtcNow;
                await _taskRepository.UpdateAsync(task);
            }
        }
    }
}

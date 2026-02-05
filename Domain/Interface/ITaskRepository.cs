using Domain.Entity;

namespace Domain.Interface
{
    public interface ITaskRepository
    {
        System.Threading.Tasks.Task<WorkTask?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task<IEnumerable<WorkTask>> GetAllAsync();
        System.Threading.Tasks.Task<IEnumerable<WorkTask>> GetByAssigneeIdAsync(Guid assigneeId);
        System.Threading.Tasks.Task<IEnumerable<WorkTask>> GetByCreatorIdAsync(Guid creatorId);
        System.Threading.Tasks.Task<WorkTask> CreateAsync(WorkTask task);
        System.Threading.Tasks.Task<WorkTask> UpdateAsync(WorkTask task);
        System.Threading.Tasks.Task<bool> DeleteAsync(Guid id);
        System.Threading.Tasks.Task<bool> ExistsAsync(Guid id);
    }
}

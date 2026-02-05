using Domain.Entity;

namespace Domain.Interface
{
    public interface ITaskRepository
    {
        Task<Entity.Task?> GetByIdAsync(Guid id);
        Task<IEnumerable<Entity.Task>> GetAllAsync();
        Task<IEnumerable<Entity.Task>> GetByAssigneeIdAsync(Guid assigneeId);
        Task<IEnumerable<Entity.Task>> GetByCreatorIdAsync(Guid creatorId);
        Task<Entity.Task> CreateAsync(Entity.Task task);
        Task<Entity.Task> UpdateAsync(Entity.Task task);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}

using Domain.Entity;

namespace Domain.Interface
{
    public interface IWorklogRepository
    {
        System.Threading.Tasks.Task<Worklog?> GetByIdAsync(Guid id);
        System.Threading.Tasks.Task<IEnumerable<Worklog>> GetAllAsync();
        System.Threading.Tasks.Task<IEnumerable<Worklog>> GetByUserIdAsync(Guid userId);
        System.Threading.Tasks.Task<IEnumerable<Worklog>> GetByTaskIdAsync(Guid taskId);
        System.Threading.Tasks.Task<IEnumerable<Worklog>> GetByUserIdAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        System.Threading.Tasks.Task<Worklog?> GetByUserTaskAndDateAsync(Guid userId, Guid taskId, DateTime date);
        System.Threading.Tasks.Task<decimal> GetTotalHoursByUserAndDateAsync(Guid userId, DateTime date);
        System.Threading.Tasks.Task<Worklog> CreateAsync(Worklog worklog);
        System.Threading.Tasks.Task<Worklog> UpdateAsync(Worklog worklog);
        System.Threading.Tasks.Task<bool> DeleteAsync(Guid id);
        System.Threading.Tasks.Task<bool> ExistsAsync(Guid userId, Guid taskId, DateTime date);
    }
}

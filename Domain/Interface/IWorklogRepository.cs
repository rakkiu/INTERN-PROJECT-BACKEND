using Domain.Entity;

namespace Domain.Interface
{
    public interface IWorklogRepository
    {
        Task<Worklog?> GetByIdAsync(Guid id);
        Task<IEnumerable<Worklog>> GetAllAsync();
        Task<IEnumerable<Worklog>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<Worklog>> GetByTaskIdAsync(Guid taskId);
        Task<IEnumerable<Worklog>> GetByUserIdAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate);
        Task<Worklog?> GetByUserTaskAndDateAsync(Guid userId, Guid taskId, DateTime date);
        Task<decimal> GetTotalHoursByUserAndDateAsync(Guid userId, DateTime date);
        Task<Worklog> CreateAsync(Worklog worklog);
        Task<Worklog> UpdateAsync(Worklog worklog);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid userId, Guid taskId, DateTime date);
    }
}

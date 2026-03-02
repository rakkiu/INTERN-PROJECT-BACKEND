using Domain.Entity;
using Domain.Interface;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class WorklogRepository : IWorklogRepository
    {
        private readonly ApplicationDbContext _context;

        public WorklogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task<Worklog?> GetByIdAsync(Guid id)
        {
            return await _context.Worklogs
                .Include(w => w.Task)
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.Id == id);
        }

        public async System.Threading.Tasks.Task<IEnumerable<Worklog>> GetAllAsync()
        {
            return await _context.Worklogs
                .Include(w => w.Task)
                .Include(w => w.User)
                .OrderByDescending(w => w.LogDate)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<Worklog>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Worklogs
                .Include(w => w.Task)
                .Where(w => w.UserId == userId)
                .OrderByDescending(w => w.LogDate)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<Worklog>> GetByTaskIdAsync(Guid taskId)
        {
            return await _context.Worklogs
                .Include(w => w.User)
                .Where(w => w.TaskId == taskId)
                .OrderByDescending(w => w.LogDate)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<Worklog>> GetByUserIdAndDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate)
        {
            return await _context.Worklogs
                .Include(w => w.Task)
                .Where(w => w.UserId == userId && w.LogDate >= startDate && w.LogDate <= endDate)
                .OrderByDescending(w => w.LogDate)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<Worklog?> GetByUserTaskAndDateAsync(Guid userId, Guid taskId, DateTime date)
        {
            return await _context.Worklogs
                .Include(w => w.Task)
                .Include(w => w.User)
                .FirstOrDefaultAsync(w => w.UserId == userId && w.TaskId == taskId && w.LogDate.Date == date.Date);
        }

        public async System.Threading.Tasks.Task<decimal> GetTotalHoursByUserAndDateAsync(Guid userId, DateTime date)
        {
            return await _context.Worklogs
                .Where(w => w.UserId == userId && w.LogDate.Date == date.Date)
                .SumAsync(w => w.HoursSpent);
        }

        public async System.Threading.Tasks.Task<Worklog> CreateAsync(Worklog worklog)
        {
            worklog.CreatedAt = DateTime.UtcNow;
            
            _context.Worklogs.Add(worklog);
            await _context.SaveChangesAsync();
            
            return worklog;
        }

        public async System.Threading.Tasks.Task<Worklog> UpdateAsync(Worklog worklog)
        {
            _context.Worklogs.Update(worklog);
            await _context.SaveChangesAsync();
            
            return worklog;
        }

        public async System.Threading.Tasks.Task<bool> DeleteAsync(Guid id)
        {
            var worklog = await _context.Worklogs.FindAsync(id);
            if (worklog == null)
                return false;

            _context.Worklogs.Remove(worklog);
            await _context.SaveChangesAsync();
            return true;
        }

        public async System.Threading.Tasks.Task<bool> ExistsAsync(Guid userId, Guid taskId, DateTime date)
        {
            return await _context.Worklogs
                .AnyAsync(w => w.UserId == userId && w.TaskId == taskId && w.LogDate.Date == date.Date);
        }
    }
}

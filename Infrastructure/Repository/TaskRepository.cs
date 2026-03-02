using Domain.Entity;
using Domain.Interface;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task<WorkTask?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.CreatedBy)
                .Include(t => t.Worklogs)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async System.Threading.Tasks.Task<IEnumerable<WorkTask>> GetAllAsync()
        {
            return await _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.CreatedBy)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<WorkTask>> GetByAssigneeIdAsync(Guid assigneeId)
        {
            return await _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.CreatedBy)
                .Where(t => t.AssigneeId == assigneeId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<IEnumerable<WorkTask>> GetByCreatorIdAsync(Guid creatorId)
        {
            return await _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.CreatedBy)
                .Where(t => t.CreatedById == creatorId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async System.Threading.Tasks.Task<WorkTask> CreateAsync(WorkTask task)
        {
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            
            return task;
        }

        public async System.Threading.Tasks.Task<WorkTask> UpdateAsync(WorkTask task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            
            return task;
        }

        public async System.Threading.Tasks.Task<bool> DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async System.Threading.Tasks.Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }
    }
}

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

        public async Task<Domain.Entity.Task?> GetByIdAsync(Guid id)
        {
            return await _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.CreatedBy)
                .Include(t => t.Worklogs)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<IEnumerable<Domain.Entity.Task>> GetAllAsync()
        {
            return await _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.CreatedBy)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entity.Task>> GetByAssigneeIdAsync(Guid assigneeId)
        {
            return await _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.CreatedBy)
                .Where(t => t.AssigneeId == assigneeId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Domain.Entity.Task>> GetByCreatorIdAsync(Guid creatorId)
        {
            return await _context.Tasks
                .Include(t => t.Assignee)
                .Include(t => t.CreatedBy)
                .Where(t => t.CreatedById == creatorId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<Domain.Entity.Task> CreateAsync(Domain.Entity.Task task)
        {
            task.CreatedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;
            
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            
            return task;
        }

        public async Task<Domain.Entity.Task> UpdateAsync(Domain.Entity.Task task)
        {
            task.UpdatedAt = DateTime.UtcNow;
            
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            
            return task;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Tasks.AnyAsync(t => t.Id == id);
        }
    }
}

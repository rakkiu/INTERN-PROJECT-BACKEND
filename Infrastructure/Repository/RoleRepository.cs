using Domain.Entity;
using Domain.Interfaces;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ApplicationDbContext _context;

        public RoleRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Role>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Roles
                .OrderBy(r => r.Name)
                .ToListAsync(ct);
        }

        public async Task<Role?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Id == id, ct);
        }

        public async Task<Role?> GetByNameAsync(string name, CancellationToken ct = default)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.Name == name, ct);
        }

        public async Task AddAsync(Role role, CancellationToken ct = default)
        {
            await _context.Roles.AddAsync(role, ct);
        }

        public void Update(Role role)
        {
            _context.Roles.Update(role);
        }

        public void Remove(Role role)
        {
            _context.Roles.Remove(role);
        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await _context.SaveChangesAsync(ct);
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Roles.AnyAsync(r => r.Id == id, ct);
        }

        public async Task<bool> RoleNameExistsAsync(string name, CancellationToken ct = default)
        {
            return await _context.Roles.AnyAsync(r => r.Name == name, ct);
        }
    }
}

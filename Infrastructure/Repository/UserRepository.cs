using Domain.Entity;
using Domain.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task AddAsync(User user, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var encryptedEmail = EncryptionHelper.EncryptDeterministic(email);
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == encryptedEmail, ct);

            return user;
        }

        public Task<User?> GetByEmailWithoutDecryptAsync(string email, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdWithoutDecryptAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetByIdWithRoleAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetListUser()
        {
            throw new NotImplementedException();
        }

        public void Remove(User user)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return _context.SaveChangesAsync(ct);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
        }

        public void UpdatePasswordOnly(User user)
        {
            _context.Entry(user).Property(u => u.PasswordHash).IsModified = true;
        }

        public void UpdateV1(User user)
        {
            _context.Users.Update(user);
        }
    }
}

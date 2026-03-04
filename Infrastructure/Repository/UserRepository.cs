using Domain.Entity;
using Domain.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
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

        public async Task AddAsync(User user, CancellationToken ct = default)
        {
            await _context.Users.AddAsync(user, ct);
        }

        public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
        {
            var encryptedEmail = EncryptionHelper.EncryptDeterministic(email);
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.RefreshTokens)
                .FirstOrDefaultAsync(u => u.Email == encryptedEmail, ct);

            // Nếu bạn có logic giải mã (Decrypt) thủ công ở Repository, hãy gọi nó ở đây.
            // Ví dụ: if (user != null) user.Email = EncryptionHelper.Decrypt(user.Email);
            return user;
        }

        public async Task<User?> GetByEmailWithoutDecryptAsync(string email, CancellationToken ct = default)
        {
            var encryptedEmail = EncryptionHelper.EncryptDeterministic(email);
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == encryptedEmail, ct);
        }

        public async Task<User?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

            // Tương tự, nếu bạn có hàm giải mã tên/số điện thoại thủ công, thêm vào đây.
            return user;
        }

        public async Task<User?> GetByIdWithoutDecryptAsync(Guid id, CancellationToken ct = default)
        {
            // Trả về thẳng dữ liệu thô từ DB (chưa qua bước giải mã nếu có)
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<User?> GetByIdWithRoleAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task<IEnumerable<User>> GetListUser()
        {
            // Dùng AsNoTracking() cho những query chỉ đọc danh sách để tăng hiệu suất
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public void Remove(User user)
        {
            _context.Users.Remove(user);
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
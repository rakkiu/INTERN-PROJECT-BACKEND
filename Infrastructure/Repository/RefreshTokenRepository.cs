using Domain.Entity;
using Domain.Interface;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async System.Threading.Tasks.Task DeleteAsync(string refreshToken)
        {
            var token = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
            }
        }

        public async System.Threading.Tasks.Task<RefreshToken?> GetAccessTokenByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.UserId == userId, ct);
            return token;
        }

        public async System.Threading.Tasks.Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(t => t.Token == token);
        }

        public async System.Threading.Tasks.Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token, ct);
        }

        public async System.Threading.Tasks.Task RemoveTokenAsync(RefreshToken token, CancellationToken ct = default)
        {
            await _context.RefreshTokens
                .Where(t => t.Id == token.Id)
                .ExecuteDeleteAsync(ct);
        }

        public async System.Threading.Tasks.Task SaveChangeAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async System.Threading.Tasks.Task SaveResetTokenAsync(RefreshToken token, CancellationToken ct = default)
        {
            token.ExpiresAt = DateTime.SpecifyKind(token.ExpiresAt, DateTimeKind.Unspecified);
            await _context.RefreshTokens.AddAsync(token, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async System.Threading.Tasks.Task SaveTokenAsync(RefreshToken token, CancellationToken ct = default)
        {
            await _context.RefreshTokens.AddAsync(token, ct);
        }

        public void Update(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
        }

        public async System.Threading.Tasks.Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            _context.RefreshTokens.Update(refreshToken);
        }
    }
}

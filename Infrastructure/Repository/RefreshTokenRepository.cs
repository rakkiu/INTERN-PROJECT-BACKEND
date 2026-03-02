using Domain.Entity;
using Domain.Interface;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(string refreshToken)
        {
            var token = _context.RefreshTokens.FirstOrDefault(t => t.Token == refreshToken);
            if (token != null)
            {
                _context.RefreshTokens.Remove(token);
            }
        }

        public async Task<RefreshToken?> GetAccessTokenByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var token = await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.UserId == userId, ct);
            return token;
        }

        public async Task<RefreshToken?> GetByTokenAsync(string token)
        {
            return await _context.RefreshTokens.Include(rt => rt.User).FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default)
        {
            return await _context.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == token, ct);
        }

        public Task<User> GetUserByRefreshTokenAsync(string refreshToken)
        {
            var user = _context.RefreshTokens
                .Where(t => t.Token == refreshToken)
                .Select(t => t.User)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<RefreshToken> GetValidRefreshToken(string refreshToken)
        {
            var token = await _context.RefreshTokens
                .Include(rt => rt.User)
                .ThenInclude(u => u.Role)
                .FirstOrDefaultAsync(t => t.Token == refreshToken && !t.IsRevoked && t.ExpiresAt > DateTime.UtcNow);
            return token;
        }

        public async Task RemoveTokenAsync(RefreshToken token, CancellationToken ct = default)
        {
            await _context.RefreshTokens
                .Where(t => t.Id == token.Id)
                .ExecuteDeleteAsync(ct);
        }

        public async Task RevokeTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens
                .Where(t => t.Id == refreshToken.Id)
                .ExecuteUpdateAsync(t => t.SetProperty(rt => rt.IsRevoked, true));
        }

        public async Task SaveChangeAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);
        }

        public async Task SaveResetTokenAsync(RefreshToken token, CancellationToken ct = default)
        {
            token.ExpiresAt = DateTime.SpecifyKind(token.ExpiresAt, DateTimeKind.Unspecified);
            await _context.RefreshTokens.AddAsync(token, ct);
            await _context.SaveChangesAsync(ct);
        }

        public async Task SaveTokenAsync(RefreshToken token, CancellationToken ct = default)
        {
            await _context.RefreshTokens.AddAsync(token, ct);
        }

        public void Update(RefreshToken token)
        {
            _context.RefreshTokens.Update(token);
        }

        public async Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
        {
            _context.RefreshTokens.Update(refreshToken);
        }
    }
}

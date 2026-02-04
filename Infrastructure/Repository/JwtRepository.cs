using Domain.Entity;
using Domain.Interface;
using Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class JwtRepository : IJwtTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public JwtRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task DeleteAsync(string refreshToken)
        {
            var token = _context.JwtTokens.FirstOrDefault(t => t.Token == refreshToken);
            if (token != null)
            {
                _context.JwtTokens.Remove(token);
                
            }
        }

        public async Task<JwtToken?> GetAccessTokenByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            var token = await _context.JwtTokens
                .FirstOrDefaultAsync(t => t.UserId == userId && t.TokenType == "AccessToken", ct);
            return token;
        }

        public async Task<JwtToken?> GetByTokenAsync(string token)
        {
            return await _context.JwtTokens.Include(rt => rt.User).FirstOrDefaultAsync(t => t.Token == token);
        }

        public async Task<JwtToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default)
        {
            return await _context.JwtTokens
                .FirstOrDefaultAsync(t => t.Token == token && t.TokenType == "RefreshToken", ct);
        }

        public async Task RemoveTokenAsync(JwtToken token, CancellationToken ct = default)
        {
           await _context.JwtTokens
                .Where(t => t.Id == token.Id)
                .ExecuteDeleteAsync(ct);
        }

        public async Task SaveChangeAsync(CancellationToken ct = default)
        {
            await  _context.SaveChangesAsync(ct);
        }

        public async Task SaveResetTokenAsync(JwtToken token, CancellationToken ct = default)
        {

            // Ensure ExpiresAt is Unspecified kind for PostgreSQL compatibility
            token.ExpiresAt = DateTime.SpecifyKind(token.ExpiresAt, DateTimeKind.Unspecified);

            await _context.JwtTokens.AddAsync(token, ct);
            await _context.SaveChangesAsync(ct);

        }

        public async Task SaveTokenAsync(JwtToken token, CancellationToken ct = default)
        {
            await _context.JwtTokens.AddAsync(token, ct);
        }

        public void Update(JwtToken token)
        {
           _context.JwtTokens.Update(token);
        }

        public async Task UpdateAsync(JwtToken refreshToken, CancellationToken cancellationToken)
        {
            _context.JwtTokens.Update(refreshToken);
        }
    }
}

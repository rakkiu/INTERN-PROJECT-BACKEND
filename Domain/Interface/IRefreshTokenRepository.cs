using Domain.Entity;

namespace Domain.Interface
{
    /// <summary>
    /// Repository interface for RefreshToken operations
    /// </summary>
    public interface IRefreshTokenRepository
    {
        System.Threading.Tasks.Task<RefreshToken?> GetByTokenAsync(string token);
        System.Threading.Tasks.Task DeleteAsync(string refreshToken);
        System.Threading.Tasks.Task SaveTokenAsync(RefreshToken token, CancellationToken ct = default);
        System.Threading.Tasks.Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default);
        System.Threading.Tasks.Task RemoveTokenAsync(RefreshToken token, CancellationToken ct = default);
        System.Threading.Tasks.Task<RefreshToken?> GetAccessTokenByUserIdAsync(Guid userId, CancellationToken ct = default);
        System.Threading.Tasks.Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        System.Threading.Tasks.Task SaveChangeAsync(CancellationToken ct = default);
        void Update(RefreshToken token);
        System.Threading.Tasks.Task SaveResetTokenAsync(RefreshToken token, CancellationToken ct = default);
    }
}

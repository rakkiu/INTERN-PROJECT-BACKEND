using Domain.Entity;

namespace Domain.Interface
{
    /// <summary>
    /// Repository interface for RefreshToken operations
    /// </summary>
    public interface IRefreshTokenRepository
    {
        Task<RefreshToken?> GetByTokenAsync(string token);
        Task DeleteAsync(string refreshToken);
        Task SaveTokenAsync(RefreshToken token, CancellationToken ct = default);
        Task<RefreshToken?> GetRefreshTokenAsync(string token, CancellationToken ct = default);
        Task RemoveTokenAsync(RefreshToken token, CancellationToken ct = default);
        Task<RefreshToken?> GetAccessTokenByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken);
        Task SaveChangeAsync(CancellationToken ct = default);
        void Update(RefreshToken token);
        Task SaveResetTokenAsync(RefreshToken token, CancellationToken ct = default);

        Task<User> GetUserByRefreshTokenAsync(string refreshToken);
        Task<RefreshToken> GetValidRefreshToken(string refreshToken);
        Task RevokeTokenAsync(RefreshToken refreshToken);
    }
}

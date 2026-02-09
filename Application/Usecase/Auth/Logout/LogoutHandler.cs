using Domain.Interface;
using MediatR;

namespace Application.Usecase.Auth.Logout
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, bool>
    {
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LogoutHandler(IRefreshTokenRepository refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<bool> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            // Get all refresh tokens of the user
            var refreshToken = await _refreshTokenRepository.GetAccessTokenByUserIdAsync(request.UserId, cancellationToken);

            if (refreshToken == null)
            {
                return true; // User has no active refresh token
            }

            // Revoke the refresh token
            await _refreshTokenRepository.RevokeTokenAsync(refreshToken);
            await _refreshTokenRepository.SaveChangeAsync(cancellationToken);

            return true;
        }
    }
}

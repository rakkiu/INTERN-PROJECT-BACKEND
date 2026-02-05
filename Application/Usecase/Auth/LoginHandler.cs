using Application.Interfaces;
using Application.Model.Auth.Login;
using Domain.Entity;
using Domain.Interface;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Auth
{
    public class LoginHandler : IRequestHandler<LoginCommand, LoginResponseDto>
    {
        private readonly IJwtService _jwt;
        private readonly IRefreshTokenRepository _refreshTokenRepo;
        private readonly IUserRepository _repo;
        
        public LoginHandler(IJwtService jwt, IRefreshTokenRepository refreshTokenRepo, IUserRepository repo)
        {
            _jwt = jwt;
            _refreshTokenRepo = refreshTokenRepo;
            _repo = repo;
        }
        
        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repo.GetByEmailAsync(request.email, cancellationToken);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.............");

            // Generate tokens
            var accessToken = _jwt.GenerateAccessToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            // Get expiration times from settings
            var refreshTokenExpirationDays = _jwt.GetRefreshTokenExpirationDays();

            // Save RefreshToken to database
            var refreshTokenEntity = new RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                IsRevoked = false,
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
                CreatedAt = DateTime.UtcNow
            };

            await _refreshTokenRepo.SaveTokenAsync(refreshTokenEntity, cancellationToken);
            await _refreshTokenRepo.SaveChangeAsync(cancellationToken);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }
    }
}


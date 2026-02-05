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
        private readonly IJwtTokenRepository _jwtTokenRepo;
        private readonly IUserRepository _repo;
        public LoginHandler(IJwtService jwt, IJwtTokenRepository jwtTokenRepo, IUserRepository repo)
        {
            _jwt = jwt;
            _jwtTokenRepo = jwtTokenRepo;
            _repo = repo;
        }
        public async Task<LoginResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _repo.GetByEmailAsync(request.email, cancellationToken);
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.............");

            //if (user.IsActive == false)
            //    throw new UnauthorizedAccessException("User account is deactivated.");

            // Generate tokens
            var accessToken = _jwt.GenerateAccessToken(user);
            var refreshToken = _jwt.GenerateRefreshToken();

            // Get expiration times from settings
            var accessTokenExpirationMinutes = _jwt.GetAccessTokenExpirationMinutes();
            var refreshTokenExpirationDays = _jwt.GetRefreshTokenExpirationDays();

            // Save AccessToken to database
            var accessTokenEntity = new JwtToken
            {
                Token = accessToken,
                TokenType = "AccessToken",
                ExpiresAt = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes),
                IsRevoked = false,
                UserId = user.Id
            };

            // Save RefreshToken to database
            var refreshTokenEntity = new JwtToken
            {
                Token = refreshToken,
                TokenType = "RefreshToken",
                ExpiresAt = DateTime.UtcNow.AddDays(refreshTokenExpirationDays),
                IsRevoked = false,
                UserId = user.Id
            };

            // Save both tokens to database
            await _jwtTokenRepo.SaveTokenAsync(accessTokenEntity, cancellationToken);
            await _jwtTokenRepo.SaveTokenAsync(refreshTokenEntity, cancellationToken);

            return new LoginResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

    }
}


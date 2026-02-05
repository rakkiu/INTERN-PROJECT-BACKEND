using Application.Interfaces;
using Application.Model.Auth.Refresh;
using Domain.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usecase.Auth.Refresh
{
    public class RefreshHandler : IRequestHandler<RefreshCommand, RefreshResponseDto>
    {
        private readonly IRefreshTokenRepository _refreshRepository;
        private readonly IJwtService _jwtService;

        public RefreshHandler(IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService)
        {
            _refreshRepository = refreshTokenRepository;
            _jwtService = jwtService;
        }
        public async Task<RefreshResponseDto> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _refreshRepository.GetValidRefreshToken(request.refreshToken);
            if (refreshToken == null)
            {
                throw new UnauthorizedAccessException("Invalid refresh token.");
            }
            if (!refreshToken.User.IsActive)
            {
                throw new UnauthorizedAccessException("User is inactive.");
            }
            await _refreshRepository.RevokeTokenAsync(refreshToken);

            var newAccessToken = _jwtService.GenerateAccessToken(refreshToken.User);

            var newRefreshToken = _jwtService.GenerateRefreshToken();

            return new RefreshResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };

        }
    }
}

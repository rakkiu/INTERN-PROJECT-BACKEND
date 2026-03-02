using Application.Model.Auth.Login;
using Application.Model.Auth.Refresh;
using Application.Model.Auth.Logout;
using Application.Usecase.Auth.Login;
using Application.Usecase.Auth.Refresh;
using Application.Usecase.Auth.Logout;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/auth")]

    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponseDto>), 200)]
        public async Task<ActionResult<ApiResponse<LoginResponseDto>>> Login([FromBody] LoginCommand command)
        {
            var res = await _mediator.Send(command);

            return Ok(new ApiResponse<LoginResponseDto>
            {
                StatusCode = 200,
                Message = "Login successful",
                Data = res,
                ResponsedAt = DateTime.UtcNow
            });

        }

        [HttpPost("refresh")]
        [ProducesResponseType(typeof(ApiResponse<RefreshResponseDto>), 200)]
        public async Task<ActionResult<ApiResponse<RefreshResponseDto>>> Refresh([FromBody] RefreshCommand command)
        {
            var res = await _mediator.Send(command);
            return Ok(new ApiResponse<RefreshResponseDto>
            {
                StatusCode = 200,
                Message = "Token refreshed successfully",
                Data = res,
                ResponsedAt = DateTime.UtcNow
            });
        }

        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(typeof(ApiResponse<LogoutResponseDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<ApiResponse<LogoutResponseDto>>> Logout()
        {
            // Extract userId from JWT token
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new ApiResponse<object>
                {
                    StatusCode = 401,
                    Message = "Invalid or missing user ID in token",
                    ResponsedAt = DateTime.UtcNow
                });
            }

            var command = new LogoutCommand(userId);
            var result = await _mediator.Send(command);

            return Ok(new ApiResponse<LogoutResponseDto>
            {
                StatusCode = 200,
                Message = "Logout successful",
                Data = new LogoutResponseDto
                {
                    Success = result,
                    Message = "You have been logged out successfully"
                },
                ResponsedAt = DateTime.UtcNow
            });
        }
    }
}

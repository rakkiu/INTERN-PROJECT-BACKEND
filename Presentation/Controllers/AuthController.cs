using Application.Model.Auth.Login;
using Application.Model.Auth.Refresh;
using Application.Usecase.Auth.Login;
using Application.Usecase.Auth.Refresh;
using MediatR;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

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
    }
}

using Application.Model.Worklog;
using Application.Usecase.Worklog.Create;
using Application.Usecase.Worklog.GetMine;
using Application.Usecase.Worklog.GetTeam;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/worklogs")]
    [Authorize]
    public class WorklogController : ControllerBase
    {
        private readonly IMediator _mediator;

        public WorklogController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Log work hours for a task (assigned member only).
        /// Business rules:
        ///   - 1 worklog per user/task/day
        ///   - 0 &lt; hoursSpent ≤ 8
        ///   - Total daily hours ≤ 8
        ///   - Cannot log for DONE tasks, future dates, or unassigned tasks
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMIN,LEADER,MEMBER")]
        [ProducesResponseType(typeof(ApiResponse<WorklogResponseDto>), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ApiResponse<WorklogResponseDto>>> CreateWorklog([FromBody] CreateWorklogRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new ApiResponse<object>
                {
                    StatusCode = 401,
                    Message = "Invalid or missing user ID in token.",
                    ResponsedAt = DateTime.UtcNow
                });
            }

            var command = new CreateWorklogCommand(userId, request.TaskId, request.Date, request.HoursSpent, request.Note);
            var result = await _mediator.Send(command);

            return StatusCode(201, new ApiResponse<WorklogResponseDto>
            {
                StatusCode = 201,
                Message = "Worklog created successfully.",
                Data = result,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Get all worklogs of the authenticated user.
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorklogResponseDto>>), 200)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorklogResponseDto>>>> GetMyWorklogs()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new ApiResponse<object>
                {
                    StatusCode = 401,
                    Message = "Invalid or missing user ID in token.",
                    ResponsedAt = DateTime.UtcNow
                });
            }

            var query = new GetMyWorklogsQuery(userId);
            var result = await _mediator.Send(query);

            return Ok(new ApiResponse<IEnumerable<WorklogResponseDto>>
            {
                StatusCode = 200,
                Message = "Worklogs retrieved successfully.",
                Data = result,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Get all worklogs across the entire team (ADMIN, LEADER only).
        /// </summary>
        [HttpGet("team")]
        [Authorize(Roles = "ADMIN,LEADER")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<WorklogResponseDto>>), 200)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ApiResponse<IEnumerable<WorklogResponseDto>>>> GetTeamWorklogs()
        {
            var query = new GetTeamWorklogsQuery();
            var result = await _mediator.Send(query);

            return Ok(new ApiResponse<IEnumerable<WorklogResponseDto>>
            {
                StatusCode = 200,
                Message = "Team worklogs retrieved successfully.",
                Data = result,
                ResponsedAt = DateTime.UtcNow
            });
        }
    }
}

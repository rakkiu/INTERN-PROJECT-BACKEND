using Application.Model.Task;
using Application.Usecase.Task.AssignTask;
using Application.Usecase.Task.CreateTask;
using Application.Usecase.Task.DeleteTask;
using Application.Usecase.Task.GetTasks;
using Application.Usecase.Task.UpdateTask;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;
using System.Security.Claims;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TaskController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all tasks (accessible to all users)
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<TaskDto>>), 200)]
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskDto>>>> GetAllTasks()
        {
            var query = new GetTasksQuery();
            var tasks = await _mediator.Send(query);

            return Ok(new ApiResponse<IEnumerable<TaskDto>>
            {
                StatusCode = 200,
                Message = "Tasks retrieved successfully",
                Data = tasks,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Create a new task (ADMIN, LEADER only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "ADMIN,LEADER")]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 201)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ApiResponse<TaskDto>>> CreateTask([FromBody] CreateTaskRequestDto request)
        {
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

            var command = new CreateTaskCommand(request.Title, request.Description, request.Deadline, request.Priority, userId);
            var task = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, new ApiResponse<TaskDto>
            {
                StatusCode = 201,
                Message = "Task created successfully",
                Data = task,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Assign task to a user (ADMIN, LEADER only)
        /// </summary>
        [HttpPost("{id}/assign")]
        [Authorize(Roles = "ADMIN,LEADER")]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ApiResponse<TaskDto>>> AssignTask(Guid id, [FromBody] AssignTaskRequestDto request)
        {
            var command = new AssignTaskCommand(id, request.AssigneeId);
            var task = await _mediator.Send(command);

            return Ok(new ApiResponse<TaskDto>
            {
                StatusCode = 200,
                Message = "Task assigned successfully",
                Data = task,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Get task by ID (accessible to all users)
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApiResponse<TaskDto>>> GetTaskById(Guid id)
        {
            var query = new GetTasksQuery();
            var tasks = await _mediator.Send(query);
            var task = tasks.FirstOrDefault(t => t.Id == id);

            if (task == null)
            {
                return NotFound(new ApiResponse<object>
                {
                    StatusCode = 404,
                    Message = "Task not found",
                    ResponsedAt = DateTime.UtcNow
                });
            }

            return Ok(new ApiResponse<TaskDto>
            {
                StatusCode = 200,
                Message = "Task retrieved successfully",
                Data = task,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Update task (ADMIN, LEADER, MEMBER - if assigned)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN,LEADER,MEMBER")]
        [ProducesResponseType(typeof(ApiResponse<TaskDto>), 200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ApiResponse<TaskDto>>> UpdateTask(Guid id, [FromBody] UpdateTaskRequestDto request)
        {
            var command = new UpdateTaskCommand(id, request.Title, request.Description, request.Deadline, request.Priority, request.Status);
            var task = await _mediator.Send(command);

            return Ok(new ApiResponse<TaskDto>
            {
                StatusCode = 200,
                Message = "Task updated successfully",
                Data = task,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Delete task (ADMIN, LEADER only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN,LEADER")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(403)]
        public async Task<ActionResult<ApiResponse<object>>> DeleteTask(Guid id)
        {
            var command = new DeleteTaskCommand(id);
            var result = await _mediator.Send(command);

            return Ok(new ApiResponse<object>
            {
                StatusCode = 200,
                Message = "Task deleted successfully",
                Data = new { Deleted = result },
                ResponsedAt = DateTime.UtcNow
            });
        }
    }
}

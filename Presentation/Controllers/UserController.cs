using Application.Model.User;
using Application.Usecase.Admin.User.Create;
using Application.Usecase.Admin.User.Delete;
using Application.Usecase.Admin.User.GetAll;
using Application.Usecase.Admin.User.Update;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/users")]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all users (Admin only)
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UserResponseDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 403)]
        public async Task<ActionResult<ApiResponse<IEnumerable<UserResponseDto>>>> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var users = await _mediator.Send(query);

            return Ok(new ApiResponse<IEnumerable<UserResponseDto>>
            {
                StatusCode = 200,
                Message = "Users retrieved successfully",
                Data = users,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Create a new user (Admin only)
        /// Password will be auto-generated and sent to user email
        /// </summary>
        /// <param name="request">User creation request</param>
        /// <returns>Created user details</returns>
        [HttpPost]
        //[Authorize(Roles = "ADMIN")]
        [ProducesResponseType(typeof(ApiResponse<UserResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        [ProducesResponseType(typeof(ApiResponse<string>), 403)]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> CreateUser([FromBody] CreateUserRequestDto request)
        {
            var command = new CreateUserCommand
            {
                Email = request.Email,
                FullName = request.FullName,
                RoleId = request.RoleId,
                IsActive = request.IsActive
            };

            var user = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetAllUsers), new ApiResponse<UserResponseDto>
            {
                StatusCode = 201,
                Message = "User created successfully. Password sent to user email.",
                Data = user,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Update an existing user (Admin only)
        /// </summary>
        /// <param name="id">User id</param>
        /// <param name="request">User update request</param>
        /// <returns>Updated user details</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(typeof(ApiResponse<UserResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        [ProducesResponseType(typeof(ApiResponse<string>), 403)]
        public async Task<ActionResult<ApiResponse<UserResponseDto>>> UpdateUser(Guid id, [FromBody] UpdateUserRequestDto request)
        {
            var command = new UpdateUserCommand
            {
                Id = id,
                FullName = request.FullName,
                RoleId = request.RoleId,
                IsActive = request.IsActive
            };

            var user = await _mediator.Send(command);

            return Ok(new ApiResponse<UserResponseDto>
            {
                StatusCode = 200,
                Message = "User updated successfully",
                Data = user,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Delete a user (Admin only)
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        [ProducesResponseType(typeof(ApiResponse<string>), 403)]
        public async Task<ActionResult<ApiResponse<string>>> DeleteUser(Guid id)
        {
            var command = new DeleteUserCommand(id);
            await _mediator.Send(command);

            return Ok(new ApiResponse<string>
            {
                StatusCode = 200,
                Message = "User deleted successfully",
                Data = "User has been removed",
                ResponsedAt = DateTime.UtcNow
            });
        }
    }
}

using Application.Model.Role;
using Application.Usecase.Role.Create;
using Application.Usecase.Role.Delete;
using Application.Usecase.Role.GetAll;
using Application.Usecase.Role.GetById;
using Application.Usecase.Role.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Common;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/roles")]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Get all roles
        /// </summary>
        /// <returns>List of all roles</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<RoleResponseDto>>), 200)]
        public async Task<ActionResult<ApiResponse<IEnumerable<RoleResponseDto>>>> GetAllRoles()
        {
            var query = new GetAllRolesQuery();
            var roles = await _mediator.Send(query);

            return Ok(new ApiResponse<IEnumerable<RoleResponseDto>>
            {
                StatusCode = 200,
                Message = "Roles retrieved successfully",
                Data = roles,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Get a role by id
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Role details</returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        public async Task<ActionResult<ApiResponse<RoleResponseDto>>> GetRoleById(Guid id)
        {
            var query = new GetRoleByIdQuery(id);
            var role = await _mediator.Send(query);

            return Ok(new ApiResponse<RoleResponseDto>
            {
                StatusCode = 200,
                Message = "Role retrieved successfully",
                Data = role,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Create a new role
        /// </summary>
        /// <param name="request">Role creation request</param>
        /// <returns>Created role details</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<RoleResponseDto>), 201)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<ActionResult<ApiResponse<RoleResponseDto>>> CreateRole([FromBody] CreateRoleRequestDto request)
        {
            var command = new CreateRoleCommand
            {
                Name = request.Name
            };

            var role = await _mediator.Send(command);

            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, new ApiResponse<RoleResponseDto>
            {
                StatusCode = 201,
                Message = "Role created successfully",
                Data = role,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Update an existing role
        /// </summary>
        /// <param name="id">Role id</param>
        /// <param name="request">Role update request</param>
        /// <returns>Updated role details</returns>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponse<RoleResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<ActionResult<ApiResponse<RoleResponseDto>>> UpdateRole(Guid id, [FromBody] UpdateRoleRequestDto request)
        {
            var command = new UpdateRoleCommand
            {
                Id = id,
                Name = request.Name
            };

            var role = await _mediator.Send(command);

            return Ok(new ApiResponse<RoleResponseDto>
            {
                StatusCode = 200,
                Message = "Role updated successfully",
                Data = role,
                ResponsedAt = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Delete a role
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Success status</returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<string>), 404)]
        [ProducesResponseType(typeof(ApiResponse<string>), 400)]
        public async Task<ActionResult<ApiResponse<string>>> DeleteRole(Guid id)
        {
            var command = new DeleteRoleCommand(id);
            await _mediator.Send(command);

            return Ok(new ApiResponse<string>
            {
                StatusCode = 200,
                Message = "Role deleted successfully",
                Data = "Role has been removed",
                ResponsedAt = DateTime.UtcNow
            });
        }
    }
}

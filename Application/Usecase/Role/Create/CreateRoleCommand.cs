using Application.Model.Role;
using MediatR;

namespace Application.Usecase.Role.Create
{
    /// <summary>
    /// Command to create a new role.
    /// </summary>
    public class CreateRoleCommand : IRequest<RoleResponseDto>
    {
        public string Name { get; set; } = null!;
    }
}

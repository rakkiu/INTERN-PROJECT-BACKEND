using Application.Model.Role;
using MediatR;

namespace Application.Usecase.Role.Update
{
    /// <summary>
    /// Command to update an existing role.
    /// </summary>
    public class UpdateRoleCommand : IRequest<RoleResponseDto>
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}

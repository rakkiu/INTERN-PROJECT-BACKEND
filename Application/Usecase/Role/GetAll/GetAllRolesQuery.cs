using Application.Model.Role;
using MediatR;

namespace Application.Usecase.Role.GetAll
{
    /// <summary>
    /// Query to get all roles.
    /// </summary>
    public class GetAllRolesQuery : IRequest<IEnumerable<RoleResponseDto>>
    {
    }
}

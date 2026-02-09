using Application.Model.Role;
using MediatR;

namespace Application.Usecase.Role.GetById
{
    /// <summary>
    /// Query to get a role by id.
    /// </summary>
    public class GetRoleByIdQuery : IRequest<RoleResponseDto>
    {
        public Guid Id { get; set; }

        public GetRoleByIdQuery(Guid id)
        {
            Id = id;
        }
    }
}

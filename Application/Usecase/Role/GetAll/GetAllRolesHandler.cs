using Application.Model.Role;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Role.GetAll
{
    /// <summary>
    /// Handler for GetAllRolesQuery.
    /// </summary>
    public class GetAllRolesHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleResponseDto>>
    {
        private readonly IRoleRepository _roleRepository;

        public GetAllRolesHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<RoleResponseDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleRepository.GetAllAsync(cancellationToken);

            return roles.Select(r => new RoleResponseDto
            {
                Id = r.Id,
                Name = r.Name,
                CreatedAt = r.CreatedAt,
                UserCount = r.Users.Count
            }).ToList();
        }
    }
}

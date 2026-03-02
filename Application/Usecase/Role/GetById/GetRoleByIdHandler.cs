using Application.Model.Role;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Role.GetById
{
    /// <summary>
    /// Handler for GetRoleByIdQuery.
    /// </summary>
    public class GetRoleByIdHandler : IRequestHandler<GetRoleByIdQuery, RoleResponseDto>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRoleByIdHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleResponseDto> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with id '{request.Id}' not found");
            }

            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                CreatedAt = role.CreatedAt,
                UserCount = role.Users.Count
            };
        }
    }
}

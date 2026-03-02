using Application.Model.Role;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Role.Update
{
    /// <summary>
    /// Handler for UpdateRoleCommand.
    /// </summary>
    public class UpdateRoleHandler : IRequestHandler<UpdateRoleCommand, RoleResponseDto>
    {
        private readonly IRoleRepository _roleRepository;

        public UpdateRoleHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleResponseDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            // Validate role name is not empty
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Role name cannot be empty");
            }

            var role = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with id '{request.Id}' not found");
            }

            // Check if new name already exists (and is different from current name)
            if (role.Name != request.Name)
            {
                var existingRole = await _roleRepository.GetByNameAsync(request.Name, cancellationToken);
                if (existingRole != null)
                {
                    throw new InvalidOperationException($"Role with name '{request.Name}' already exists");
                }
            }

            role.Name = request.Name.ToUpper().Trim();
            _roleRepository.Update(role);
            await _roleRepository.SaveChangesAsync(cancellationToken);

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

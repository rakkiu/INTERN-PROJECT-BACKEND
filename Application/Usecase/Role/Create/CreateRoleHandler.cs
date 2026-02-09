using Application.Model.Role;
using Domain.Entity;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Role.Create
{
    /// <summary>
    /// Handler for CreateRoleCommand.
    /// </summary>
    public class CreateRoleHandler : IRequestHandler<CreateRoleCommand, RoleResponseDto>
    {
        private readonly IRoleRepository _roleRepository;

        public CreateRoleHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<RoleResponseDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
        {
            // Validate role name is not empty
            if (string.IsNullOrWhiteSpace(request.Name))
            {
                throw new ArgumentException("Role name cannot be empty");
            }

            // Check if role name already exists
            var existingRole = await _roleRepository.GetByNameAsync(request.Name, cancellationToken);
            if (existingRole != null)
            {
                throw new InvalidOperationException($"Role with name '{request.Name}' already exists");
            }

            var role = new Domain.Entity.Role
            {
                Id = Guid.NewGuid(),
                Name = request.Name.ToUpper().Trim(),
                CreatedAt = DateTime.UtcNow
            };

            await _roleRepository.AddAsync(role, cancellationToken);
            await _roleRepository.SaveChangesAsync(cancellationToken);

            return new RoleResponseDto
            {
                Id = role.Id,
                Name = role.Name,
                CreatedAt = role.CreatedAt,
                UserCount = 0
            };
        }
    }
}

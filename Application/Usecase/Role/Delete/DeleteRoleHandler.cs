using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Role.Delete
{
    /// <summary>
    /// Handler for DeleteRoleCommand.
    /// </summary>
    public class DeleteRoleHandler : IRequestHandler<DeleteRoleCommand, bool>
    {
        private readonly IRoleRepository _roleRepository;

        public DeleteRoleHandler(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
        {
            var role = await _roleRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (role == null)
            {
                throw new KeyNotFoundException($"Role with id '{request.Id}' not found");
            }

            // Check if role has users assigned
            if (role.Users.Any())
            {
                throw new InvalidOperationException($"Cannot delete role '{role.Name}' because it has {role.Users.Count} user(s) assigned");
            }

            _roleRepository.Remove(role);
            await _roleRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

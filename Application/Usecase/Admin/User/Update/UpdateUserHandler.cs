using Application.Interfaces;
using Application.Model.User;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Admin.User.Update
{
    /// <summary>
    /// Handler for UpdateUserCommand.
    /// </summary>
    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserResponseDto>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEncryptionService _encryptionService;

        public UpdateUserHandler(IUserRepository userRepository, IRoleRepository roleRepository, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _encryptionService = encryptionService;
        }

        public async Task<UserResponseDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id '{request.Id}' not found");
            }

            // Update FullName if provided
            if (!string.IsNullOrWhiteSpace(request.FullName))
            {
                user.FullName = request.FullName.Trim();
            }

            // Update RoleId if provided and validate role exists
            if (request.RoleId.HasValue && request.RoleId.Value != user.RoleId)
            {
                var role = await _roleRepository.GetByIdAsync(request.RoleId.Value, cancellationToken);
                if (role == null)
                {
                    throw new KeyNotFoundException($"Role with id '{request.RoleId.Value}' not found");
                }
                user.RoleId = request.RoleId.Value;
            }

            // Update IsActive if provided
            if (request.IsActive.HasValue)
            {
                user.IsActive = request.IsActive.Value;
            }

            user.UpdatedAt = DateTime.UtcNow;
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return new UserResponseDto
            {
                Id = user.Id,
                Email = _encryptionService.DecryptDeterministic(user.Email),
                FullName = user.FullName,
                IsActive = user.IsActive,
                RoleName = user.Role.Name,
                RoleId = user.RoleId,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}

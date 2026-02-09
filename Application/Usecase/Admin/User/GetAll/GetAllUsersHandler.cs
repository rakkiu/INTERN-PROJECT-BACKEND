using Application.Interfaces;
using Application.Model.User;
using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Admin.User.GetAll
{
    /// <summary>
    /// Handler for GetAllUsersQuery.
    /// </summary>
    public class GetAllUsersHandler : IRequestHandler<GetAllUsersQuery, IEnumerable<UserResponseDto>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;

        public GetAllUsersHandler(IUserRepository userRepository, IEncryptionService encryptionService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
        }

        public async Task<IEnumerable<UserResponseDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _userRepository.GetListUser(cancellationToken);

            return users.Select(u => new UserResponseDto
            {
                Id = u.Id,
                Email = _encryptionService.DecryptDeterministic(u.Email),
                FullName = u.FullName,
                IsActive = u.IsActive,
                RoleName = u.Role.Name,
                RoleId = u.RoleId,
                CreatedAt = u.CreatedAt,
                UpdatedAt = u.UpdatedAt
            }).ToList();
        }
    }
}

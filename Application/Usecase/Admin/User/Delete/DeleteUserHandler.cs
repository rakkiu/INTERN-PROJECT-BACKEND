using Domain.Interfaces;
using MediatR;

namespace Application.Usecase.Admin.User.Delete
{
    /// <summary>
    /// Handler for DeleteUserCommand.
    /// </summary>
    public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IUserRepository _userRepository;

        public DeleteUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
            
            if (user == null)
            {
                throw new KeyNotFoundException($"User with id '{request.Id}' not found");
            }

            _userRepository.Remove(user);
            await _userRepository.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

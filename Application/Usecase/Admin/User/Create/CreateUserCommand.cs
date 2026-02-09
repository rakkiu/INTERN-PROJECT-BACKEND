using Application.Model.User;
using MediatR;

namespace Application.Usecase.Admin.User.Create
{
    /// <summary>
    /// Command to create a new user (Admin only).
    /// Password is auto-generated and sent to user email.
    /// </summary>
    public class CreateUserCommand : IRequest<UserResponseDto>
    {
        public string Email { get; set; } = null!;
        public string? FullName { get; set; }
        public Guid RoleId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}

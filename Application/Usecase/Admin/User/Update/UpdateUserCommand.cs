using Application.Model.User;
using MediatR;

namespace Application.Usecase.Admin.User.Update
{
    /// <summary>
    /// Command to update an existing user (Admin only).
    /// </summary>
    public class UpdateUserCommand : IRequest<UserResponseDto>
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public Guid? RoleId { get; set; }
        public bool? IsActive { get; set; }
    }
}

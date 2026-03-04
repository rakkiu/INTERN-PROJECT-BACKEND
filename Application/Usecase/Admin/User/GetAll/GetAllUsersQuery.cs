using Application.Model.User;
using MediatR;

namespace Application.Usecase.Admin.User.GetAll
{
    /// <summary>
    /// Query to get all users (Admin only).
    /// </summary>
    public class GetAllUsersQuery : IRequest<IEnumerable<UserResponseDto>>
    {
    }
}

using MediatR;

namespace Application.Usecase.Auth.Logout
{
    public record LogoutCommand(Guid UserId) : IRequest<bool>;
}

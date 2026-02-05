using Application.Model.Auth.Refresh;
using MediatR;

namespace Application.Usecase.Auth.Refresh
{
    public record RefreshCommand(string refreshToken) : IRequest<RefreshResponseDto>;

}

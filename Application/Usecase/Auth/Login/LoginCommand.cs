using Application.Model.Auth.Login;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Usecase.Auth.Login
{
    public record LoginCommand(string email, string password): IRequest<LoginResponseDto>;
    
}

using Domain.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Auth.Commands.Login
{
    public class AuthLoginCommand : IRequest<Response<AuthLoginDto>>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

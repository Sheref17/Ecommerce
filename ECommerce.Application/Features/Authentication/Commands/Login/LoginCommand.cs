using ECommerce.Application.Features.Authentication.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Authentication.Commands.Login
{
    public record LoginCommand( string Email,string Password) : IRequest<LoginResponse>;
}

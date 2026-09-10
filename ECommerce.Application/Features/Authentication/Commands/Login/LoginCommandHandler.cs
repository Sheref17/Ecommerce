using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Features.Authentication.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IIdentityService _identityService;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IIdentityService identityService,ITokenService tokenService)
        {
            _identityService = identityService;
            _tokenService = tokenService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request,CancellationToken cancellationToken)
        {
            var result = await _identityService.ValidateCredentialsAsync(request.Email,request.Password,
                cancellationToken);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors));
            }

            var accessToken = await _tokenService
                .GenerateAccessTokenAsync(result.UserId,cancellationToken);
            return  new LoginResponse(accessToken);
        }

      
    }
}

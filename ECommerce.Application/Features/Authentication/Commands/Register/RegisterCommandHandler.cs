using ECommerce.Application.Abstractions.Repositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Authentication.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, int>
    {
        private readonly IIdentityService _identityService;

        public RegisterCommandHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<int> Handle(RegisterCommand request,CancellationToken cancellationToken)
        {
            var result = await _identityService.CreateUserAsync(
                request.FirstName,
                request.LastName,
                request.Email,
                request.Password,
                cancellationToken);

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(string.Join(", ", result.Errors));
            }
            var roleAdded = await _identityService.AddToRoleAsync(result.UserId,"Customer",
                cancellationToken);

            if (!roleAdded)
            {
                throw new InvalidOperationException( "Failed to assign customer role.");
            }

            return result.UserId;
        }
    }
}

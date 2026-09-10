using ECommerce.Application.Features.Authentication.Commands.Login;
using ECommerce.Application.Features.Authentication.Commands.Register;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _sender;

        public AuthenticationController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterCommand command,
            CancellationToken cancellationToken)
        {
            var userId = await _sender.Send(command,cancellationToken);

            return Ok(userId);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginCommand command,
            CancellationToken cancellationToken)
        {
            var token = await _sender.Send(command,cancellationToken);

            return Ok(token);
        }
    }
}

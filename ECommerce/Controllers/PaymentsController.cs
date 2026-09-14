using ECommerce.Application.Features.Payments.Commands.CreatePayment;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ISender _sender;

        public PaymentsController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreatePaymentCommand command,
            CancellationToken cancellationToken)
        {
            var paymentId = await _sender.Send(command,cancellationToken);

            return Ok(paymentId);
        }
    }
}

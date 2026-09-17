using ECommerce.Application.Features.Payments.Commands.WebHook;
using ECommerce.Application.Features.Payments.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly ISender _sender;

        public PaymentController(ISender sender)
        {
            _sender = sender;
        }

        [AllowAnonymous]
        [HttpPost("webhook")]
        public async Task<IActionResult> Webhook(PaymobWebhookRequest request,
          CancellationToken cancellationToken)
        {
            request.Hmac = Request.Query["hmac"].ToString();
            await _sender.Send(new WebHookCommand(request), cancellationToken);
            return NoContent();
        }
    }
}

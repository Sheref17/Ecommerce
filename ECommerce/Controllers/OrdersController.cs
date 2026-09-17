using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Features.Orders.Commands.CancelOrder;
using ECommerce.Application.Features.Orders.Commands.Checkout;
using ECommerce.Application.Features.Orders.Commands.ConfirmOrder;
using ECommerce.Application.Features.Orders.Commands.DeliverOrder;
using ECommerce.Application.Features.Orders.Commands.ShipOrder;
using ECommerce.Application.Features.Orders.Commands.StartProcessingOrder;
using ECommerce.Application.Features.Orders.DTOs;
using ECommerce.Application.Features.Orders.Queries.GetOrderById;
using ECommerce.Application.Features.Payments.Commands.WebHook;
using ECommerce.Application.Features.Payments.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.IRepositories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ISender _sender;
   
        public OrdersController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize]
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout(CheckOutRequest request ,CancellationToken cancellationToken)
        {
            
            var orderId = await _sender.Send(new CheckoutCommand(request),cancellationToken);

            return Ok(orderId);
        }

        [Authorize]
        [HttpGet("myOrders")]
        public async Task<IActionResult> GetMyOrders(CancellationToken cancellationToken)
        {
            var orders = await _sender.Send(new GetOrdersQuery(),cancellationToken);

            return Ok(orders);
        }

        [Authorize]
        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id,CancellationToken cancellationToken)
        {
            var order = await _sender.Send(new GetOrderByIdQuery(id),
                cancellationToken);

            if (order is null)
                return NotFound();

            return Ok(order);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:Guid}/start-processing")]
        public async Task<IActionResult> StartProcessing(Guid id, CancellationToken cancellationToken)
        {
            await _sender.Send(new StartProcessingOrderCommand(id), cancellationToken);

            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("{id:Guid}/ship")]
        public async Task<IActionResult> Ship(Guid id,CancellationToken cancellationToken)
        {
            await _sender.Send(new ShipOrderCommand(id),cancellationToken);

            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:Guid}/deliver")]
        public async Task<IActionResult> Deliver(Guid id,CancellationToken cancellationToken)
        {
            await _sender.Send(new DeliverOrderCommand(id),cancellationToken);

            return NoContent();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:Guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id,CancellationToken cancellationToken)
        {
            await _sender.Send(new CancelOrderCommand(id),  cancellationToken);

            return NoContent();
        }

    }
}

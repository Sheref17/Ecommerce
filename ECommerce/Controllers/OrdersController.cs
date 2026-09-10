using ECommerce.Application.Features.Orders.Commands.CancelOrder;
using ECommerce.Application.Features.Orders.Commands.ConfirmOrder;
using ECommerce.Application.Features.Orders.Commands.CreateOrder.CreateOrderCommand;
using ECommerce.Application.Features.Orders.Commands.DeliverOrder;
using ECommerce.Application.Features.Orders.Commands.ShipOrder;
using ECommerce.Application.Features.Orders.Commands.StartProcessingOrder;
using ECommerce.Application.Features.Orders.Queries.GetOrderById;
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
        [HttpPost]
        public async Task<IActionResult> Create( CreateOrderCommand command,
            CancellationToken cancellationToken)
        {
            var orderId = await _sender.Send(command,cancellationToken);

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
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var order = await _sender.Send(new GetOrderByIdQuery(id),
                cancellationToken);

            if (order is null)
                return NotFound();

            return Ok(order);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}/confirm")]
        public async Task<IActionResult> Confirm(int id,CancellationToken cancellationToken)
        {
            await _sender.Send(new ConfirmOrderCommand(id),cancellationToken);

            return NoContent();
        }

        [HttpPut("{id:int}/start-processing")]
        public async Task<IActionResult> StartProcessing(int id,CancellationToken cancellationToken)
        {
            await _sender.Send(new StartProcessingOrderCommand(id),cancellationToken);

            return NoContent();
        }

        [HttpPut("{id:int}/ship")]
        public async Task<IActionResult> Ship(int id,CancellationToken cancellationToken)
        {
            await _sender.Send(new ShipOrderCommand(id),cancellationToken);

            return NoContent();
        }

        [HttpPut("{id:int}/deliver")]
        public async Task<IActionResult> Deliver( int id,CancellationToken cancellationToken)
        {
            await _sender.Send(new DeliverOrderCommand(id),cancellationToken);

            return NoContent();
        }

        [HttpPut("{id:int}/cancel")]
        public async Task<IActionResult> Cancel(int id,CancellationToken cancellationToken)
        {
            await _sender.Send(new CancelOrderCommand(id),  cancellationToken);

            return NoContent();
        }
    }
}

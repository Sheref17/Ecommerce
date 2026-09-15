using ECommerce.Application.Features.Baskets.Commands.AddItemToBasket;
using ECommerce.Application.Features.Baskets.Commands.ClearBasket;
using ECommerce.Application.Features.Baskets.Commands.RemoveItemFromBasket;
using ECommerce.Application.Features.Baskets.Commands.UpdateBasketItemQuantity;
using ECommerce.Application.Features.Baskets.Queries.GetBasketByUserId;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly ISender _sender;

        public BasketsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem(AddItemToBasketCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(command,cancellationToken);

            return Ok(new
            {
                message = "Item Added Successfully.",
            });
        }
        
        [HttpGet("basket")]
        public async Task<IActionResult> GetMyBasket(CancellationToken cancellationToken)
        {
            var basket = await _sender.Send(new GetBasketQuery(),cancellationToken);

            if (basket is null)
                return NotFound();

            return Ok(basket);
        }

        [HttpDelete("items")]
        public async Task<IActionResult> RemoveItem(RemoveItemFromBasketCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(command,cancellationToken);

            return Ok(new
            {
                message = "Item Deleted Successfully.",
            });
        }

        [HttpPut("items")]
        public async Task<IActionResult> UpdateItemQuantity(UpdateBasketItemQuantityCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(command,cancellationToken);

            return Ok(new
            {
                message = "Item Updated Successfully.",
            });
        }

        [HttpDelete]
        public async Task<IActionResult> Clear(ClearBasketCommand command,
            CancellationToken cancellationToken)
        {
            await _sender.Send(command,cancellationToken);

            return Ok(new
            {
                message = "Basket Deleted Successfully.",
            });
        }
    }
}

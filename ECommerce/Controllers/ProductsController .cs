using ECommerce.Application.Features.Products.Commands.CreateProduct;
using ECommerce.Application.Features.Products.Queries.GetProductById;
using ECommerce.Application.Features.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ISender _sender;
        public ProductsController(ISender sender)
        {
            _sender = sender;
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductCommand command)
        {
            var productId = await _sender.Send(command);

            return Ok(productId);
        }
        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductFilter filter, CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new GetProductsQuery(filter),cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var product = await _sender.Send(new GetProductByIdQuery(id),
                cancellationToken);

            if (product is null)
                return NotFound();

            return Ok(product);
        }
    }
}

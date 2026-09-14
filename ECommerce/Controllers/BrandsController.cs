using ECommerce.Application.Features.Brands.Commands.CreateBrand;
using ECommerce.Application.Features.Brands.Queries.GetBrandById;
using ECommerce.Application.Features.Brands.Queries.GetBrands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private readonly ISender _sender;

        public BrandsController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandCommand command,CancellationToken cancellationToken)
        {
            var brandId = await _sender.Send(command,cancellationToken);

            return Ok(brandId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll( [FromQuery] BrandFilter filter,
            CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetBrandsQuery(filter),cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:Guid}")]
        public async Task<IActionResult> GetById(Guid id,CancellationToken cancellationToken)
        {
            var brand = await _sender.Send(new GetBrandByIdQuery(id), cancellationToken);

            if (brand is null)
                return NotFound();

            return Ok(brand);
        }
    }
}

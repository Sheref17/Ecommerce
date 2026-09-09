using ECommerce.Application.Features.Categories.Commands.CreateCategory;
using ECommerce.Application.Features.Categories.Queries.GetCategories;
using ECommerce.Application.Features.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ISender _sender;
        public CategoriesController(ISender sender)
        {
            _sender = sender;
        }

        [HttpPost]
        public async Task<IActionResult> Create( CreateCategoryCommand command)
        {
            var categoryId = await _sender.Send(command);

            return Ok(categoryId);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CategoryFilter filter
            , CancellationToken cancellationToken = default)
        {
            var result = await _sender.Send(new GetCategoriesQuery(filter),cancellationToken);

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id,CancellationToken cancellationToken)
        {
            var category = await _sender.Send(new GetCategoryByIdQuery(id),cancellationToken);

            if (category is null)
                return NotFound();

            return Ok(category);
        }
    }
}

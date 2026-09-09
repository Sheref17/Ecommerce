using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Features.Categories.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse?>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public GetCategoryByIdQueryHandler(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<CategoryResponse?> Handle(GetCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _categoryReadRepository.GetByIdAsync(request.Id,cancellationToken);
        }
    }
}

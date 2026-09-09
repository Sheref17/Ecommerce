using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Common;
using ECommerce.Application.Features.Categories.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler: IRequestHandler<GetCategoriesQuery,
        PagedResult<CategoryResponse>>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;

        public GetCategoriesQueryHandler(ICategoryReadRepository categoryReadRepository)
        {
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<PagedResult<CategoryResponse>> Handle(GetCategoriesQuery request,
            CancellationToken cancellationToken)
        {
            return await _categoryReadRepository.GetAllAsync(request.Filter,cancellationToken);
        }
    }
}

using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Features.Categories.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CategoryResponse?>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICacheService _cacheService;

        public GetCategoryByIdQueryHandler(ICategoryReadRepository categoryReadRepository,
            ICacheService cacheService)
        {
            _categoryReadRepository = categoryReadRepository;
            _cacheService = cacheService;
        }

        public async Task<CategoryResponse?> Handle(GetCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"category:{request.Id}";

            var cachedCategory = await _cacheService.GetAsync(cacheKey,cancellationToken);
            if (cachedCategory is not null)
            {
                return JsonSerializer.Deserialize<CategoryResponse>(cachedCategory);
            }
           var category =await _categoryReadRepository
                .GetByIdAsync(request.Id,cancellationToken);
            if (category is null)
            {
                return null;
            }
            var serializedCategory = JsonSerializer.Serialize(category);

            await _cacheService.SetAsync(cacheKey,serializedCategory,
                TimeSpan.FromMinutes(10),cancellationToken);
            return category;

        }
    }
}

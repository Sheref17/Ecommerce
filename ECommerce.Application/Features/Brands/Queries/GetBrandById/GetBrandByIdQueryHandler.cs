using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Features.Brands.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Brands.Queries.GetBrandById
{
    public class GetBrandByIdQueryHandler : IRequestHandler<GetBrandByIdQuery, BrandResponse?>
    {
        private readonly IBrandReadRepository _brandReadRepository;
        private readonly ICacheService _cacheService;

        public GetBrandByIdQueryHandler(IBrandReadRepository brandReadRepository ,
            ICacheService cacheService)
        {
            _brandReadRepository = brandReadRepository;
            _cacheService = cacheService;
        }

        public async Task<BrandResponse?> Handle( GetBrandByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"brand:{request.Id}";
            var cachedBrand = await _cacheService.GetAsync(cacheKey , cancellationToken);
            if (cachedBrand is not null)
            {
                return JsonSerializer.Deserialize<BrandResponse>(cachedBrand);
            }
            var brand = await _brandReadRepository.GetByIdAsync(request.Id,cancellationToken);
            if (brand is null)
            {
                return null;
            }
            var serializedBrand = JsonSerializer.Serialize(brand);
            await _cacheService.SetAsync(cacheKey,serializedBrand
                ,TimeSpan.FromMinutes(10),cancellationToken);
            return brand;
        }
    }
}

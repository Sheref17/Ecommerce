using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Features.Products.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IProductReadRepository _productRepository;
        private readonly ICacheService _cacheService;
        public GetProductByIdQueryHandler(IProductReadRepository repository ,
            ICacheService cacheService
            )
        {
            _productRepository = repository;
            _cacheService = cacheService;
        }
        public async Task<ProductResponse> Handle(GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cacheKey = $"product:{request.Id}";
            var cachedProduct = await _cacheService.GetAsync(cacheKey,cancellationToken);
            if (cachedProduct is not null)
            {
                return JsonSerializer.Deserialize<ProductResponse>(cachedProduct)!;
            }
            var product =  await _productRepository
                .GetProductById(request.Id , cancellationToken);
            if(product is null)
            {
                return null;
            }
            var serializedProduct = JsonSerializer.Serialize(product);
            await _cacheService.SetAsync(cacheKey, serializedProduct,
                TimeSpan.FromMinutes(10), cancellationToken);
            return product;

        }
    }
}

using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Common;
using ECommerce.Application.Features.Products.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, PagedResult<ProductResponse>>
    {
        private readonly IProductReadRepository _productReadRepository;
        public GetProductsQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;

        }
        public async Task<PagedResult<ProductResponse>> Handle(GetProductsQuery request,
            CancellationToken cancellationToken)
        {
            return await _productReadRepository.GetAllProudcts(request.Filter , cancellationToken);
        }
    }
}

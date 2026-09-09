using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Features.Products.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductResponse>
    {
        private readonly IProductReadRepository _productRepository;
        public GetProductByIdQueryHandler(IProductReadRepository repository)
        {
            _productRepository = repository;
        }
        public async Task<ProductResponse> Handle(GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _productRepository.GetProductById(request.Id , cancellationToken);
        }
    }
}

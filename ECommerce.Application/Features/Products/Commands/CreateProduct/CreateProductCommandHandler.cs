using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepositories;
using ECommerce.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateProductCommandHandler(IProductRepository productRepository , IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var price = Money.Create(request.Price, request.Currency);
            var product = Product.Create(request.Name, request.Description, price, request.Stock, 
                request.CategoryId, request.BrandId );
            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            return product.Id;
        }
    }
}

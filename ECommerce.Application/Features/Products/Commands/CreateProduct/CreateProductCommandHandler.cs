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
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CreateProductCommandHandler(IProductRepository productRepository
            , IUnitOfWork unitOfWork
            , ICategoryRepository categoryRepository
            , IBrandRepository brandRepository
            )
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _brandRepository = brandRepository;
        }
        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId, cancellationToken);
            if(category is null)
                throw new KeyNotFoundException($"Category with id {request.CategoryId} not found.");

            var brand = await _brandRepository.GetByIdAsync(request.BrandId, cancellationToken);
            if(brand is null)
                throw new KeyNotFoundException($"Brand with id {request.BrandId} not found.");

            var price = Money.Create(request.Price, request.Currency);
            var product = Product.Create(request.Name, request.Description, price, request.Stock, 
                request.CategoryId, request.BrandId);

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return product.Id;
        }
    }
}

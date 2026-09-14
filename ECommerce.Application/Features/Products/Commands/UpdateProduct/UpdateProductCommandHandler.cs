using ECommerce.Application.Abstractions.Services;
using ECommerce.Domain.IRepositories;
using ECommerce.Domain.ValueObjects;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Products.Commands.UpdateProduct
{

    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICacheService _cacheService;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            IUnitOfWork unitOfWork,
            ICacheService cacheService)
        {
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _cacheService = cacheService;
        }

        public async Task Handle(UpdateProductCommand request,
            CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.id,
                cancellationToken);

            if (product is null)
            {
                throw new KeyNotFoundException(
                    $"Product with id {request.id} not found.");
            }
            var price = Money.Create(request.price,request.Currency);

            product.Update(request.name,request.description,price);

            await _unitOfWork.SaveChangesAsync();

            await _cacheService.RemoveAsync($"product:{request.id}",cancellationToken);
        }
    }
}

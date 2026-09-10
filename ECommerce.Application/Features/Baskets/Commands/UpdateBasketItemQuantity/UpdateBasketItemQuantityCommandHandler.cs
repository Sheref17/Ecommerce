using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Commands.UpdateBasketItemQuantity
{
    public class UpdateBasketItemQuantityCommandHandler 
        : IRequestHandler<UpdateBasketItemQuantityCommand>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBasketItemQuantityCommandHandler(
            IBasketRepository basketRepository,
            IProductRepository productRepository,
            IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateBasketItemQuantityCommand request,
            CancellationToken cancellationToken)
        {
            var basket = await _basketRepository.GetByUserIdAsync(request.UserId,
                cancellationToken);

            if (basket is null)
                throw new KeyNotFoundException(
                    $"Basket for user {request.UserId} was not found.");

            var product = await _productRepository.GetByIdAsync(request.ProductId,
                cancellationToken);

            if (product is null)
                throw new KeyNotFoundException(
                    $"Product with id {request.ProductId} was not found.");

            if (!product.IsActive)
                throw new InvalidOperationException(
                    $"Product '{product.Name}' is not active.");

            if (product.Stock < request.Quantity)
                throw new InvalidOperationException(
                    $"Insufficient stock for product '{product.Name}'.");

            basket.UpdateItemQuantity(request.ProductId,request.Quantity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}

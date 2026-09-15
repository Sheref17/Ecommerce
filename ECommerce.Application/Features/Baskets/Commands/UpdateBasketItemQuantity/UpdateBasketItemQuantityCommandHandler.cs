using ECommerce.Application.Abstractions.Services;
using ECommerce.Domain.Exceptions;
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
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBasketItemQuantityCommandHandler(
            IBasketRepository basketRepository,
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateBasketItemQuantityCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
                throw new UnauthorizedAccessException($"User is not authenticated.");

            var basket = await _basketRepository.GetByUserIdAsync(userId.Value,
                cancellationToken);
           

            if (basket is null)
                throw new KeyNotFoundException(
                    $"Basket for user {userId} was not found.");

            var product = await _productRepository.GetByIdAsync(request.ProductId,
                cancellationToken);

            if (product is null)
                throw new KeyNotFoundException(
                    $"Product with id {request.ProductId} was not found.");

            if (!product.IsActive)
                throw new DomainException(
                    $"Product '{product.Name}' is not active.");

            if (product.Stock < request.Quantity)
                throw new DomainException(
                    $"Insufficient stock for product '{product.Name}'.");

            basket.UpdateItemQuantity(request.ProductId,request.Quantity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}

using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Baskets.Commands.AddItemToBasket
{
    public class AddItemToBasketCommandHandler : IRequestHandler<AddItemToBasketCommand>
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public AddItemToBasketCommandHandler(
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

        public async Task Handle(AddItemToBasketCommand request,
            CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;
            if (userId is null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
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

            var basket = await _basketRepository.GetByUserIdAsync(userId.Value,
                cancellationToken);

            if (basket is null)
            {
                basket = Basket.Create(userId.Value);

                await _basketRepository.AddAsync(basket,cancellationToken);
            }

            basket.AddItem(request.ProductId,request.Quantity);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}

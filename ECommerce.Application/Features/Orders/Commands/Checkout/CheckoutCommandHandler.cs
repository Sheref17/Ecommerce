using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Application.Abstractions.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Orders.Commands.Checkout
{
    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, Guid>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CheckoutCommandHandler(
            ICurrentUserService currentUserService,
            IBasketRepository basketRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CheckoutCommand request,
            CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userId = _currentUserService.UserId;


                if (userId is null)
                {
                    throw new UnauthorizedAccessException("User is not authenticated.");
                }
                var basket = await _basketRepository.GetByUserIdAsync(userId.Value,
                    cancellationToken);

                if (basket is null)
                {
                    throw new KeyNotFoundException("Basket not found.");
                }

                if (!basket.Items.Any())
                {
                    throw new DomainException("Basket is empty.");
                }

                var order = Order.Create(userId.Value);
                var productIds = basket.Items.Select(x => x.ProductId).Distinct().ToList();
                var products = await _productRepository.GetByIdsAsync(productIds,cancellationToken);
                var productsById = products.ToDictionary(x => x.Id);



                foreach (var item in basket.Items)
                {
                    if (!productsById.TryGetValue(item.ProductId, out var product))
                    {
                        throw new KeyNotFoundException(
                            $"Product with id {item.ProductId} not found.");
                    }

                    if (product.Stock < item.Quantity)
                    {
                        throw new DomainException(
                            $"Not enough stock for product {product.Name}.");
                    }

                    order.AddItem(product.Id, item.Quantity, product.Price.Amount);
                    product.DecreaseStock(item.Quantity);
                }

                await _orderRepository.AddAsync(order, cancellationToken);
                basket.Clear();
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);

                return order.Id;

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
           
        }
    }
}

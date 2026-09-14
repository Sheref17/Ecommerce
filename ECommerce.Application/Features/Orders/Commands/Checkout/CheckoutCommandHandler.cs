using ECommerce.Application.Abstractions.Services;
using ECommerce.Domain.Entities;
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
            var userId = _currentUserService.UserId;


            if (userId is null)
            {
                throw new UnauthorizedAccessException( "User is not authenticated.");
            }
            var basket = await _basketRepository.GetByUserIdAsync(userId.Value,
                cancellationToken);

            if (basket is null)
            {
                throw new InvalidOperationException("Basket not found.");
            }

            if (!basket.Items.Any())
            {
                throw new InvalidOperationException("Basket is empty.");
            }

            var order = Order.Create(userId.Value);


            foreach (var item in basket.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId,
                    cancellationToken);

                if (product is null)
                {
                    throw new InvalidOperationException(
                        $"Product with id {item.ProductId} not found.");
                }
                if (product.Stock < item.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Not enough stock for product {product.Name}.");
                }

                order.AddItem(product.Id,item.Quantity,product.Price.Amount);
                product.DecreaseStock(item.Quantity);
            }

            await _orderRepository.AddAsync(order,cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            basket.Clear();

            await _unitOfWork.SaveChangesAsync();
            return order.Id;
        }
    }
}

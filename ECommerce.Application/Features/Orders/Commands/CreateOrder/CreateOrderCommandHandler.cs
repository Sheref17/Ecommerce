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

namespace ECommerce.Application.Features.Orders.Commands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderCommandHandler(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICurrentUserService userService,
            IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _currentUserService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateOrderCommand request,CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            var order = Order.Create(userId.Value);
            var productIds = request.Items.Select(x => x.ProductId).Distinct().ToList();
            var products = await _productRepository.GetByIdsAsync(productIds, cancellationToken);
            var productsById = products.ToDictionary(x => x.Id);


            foreach (var requestItem in request.Items)
            {
                if (!productsById.TryGetValue(requestItem.ProductId, out var product))
                {
                    throw new KeyNotFoundException(
                        $"Product with id {requestItem.ProductId} not found.");
                }
  

                if (!product.IsActive)
                    throw new DomainException($"Product '{product.Name}' is not active.");

                if (product.Stock < requestItem.Quantity)
                    throw new DomainException(
                        $"Insufficient stock for product '{product.Name}'.");

                order.AddItem(product.Id, requestItem.Quantity,product.Price.Amount);
                product.DecreaseStock(requestItem.Quantity);
            }
         

            await _orderRepository.AddAsync(order, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return order.Id;
        }
    }
}

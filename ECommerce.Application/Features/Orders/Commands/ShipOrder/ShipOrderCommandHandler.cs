using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Orders.Commands.ShipOrder
{
    public class ShipOrderCommandHandler : IRequestHandler<ShipOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ShipOrderCommandHandler(IOrderRepository orderRepository,IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(ShipOrderCommand request,CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId,cancellationToken);

            if (order is null)
                throw new KeyNotFoundException(
                    $"Order with id {request.OrderId} was not found.");

            order.Ship();

            await _unitOfWork.SaveChangesAsync();
        }
    }
}

using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CancelOrderCommandHandler(IOrderRepository orderRepository,IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(CancelOrderCommand request,CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.OrderId,cancellationToken);

            if (order is null)
                throw new KeyNotFoundException(
                    $"Order with id {request.OrderId} was not found.");

            order.Cancel();

            await _unitOfWork.SaveChangesAsync();
        }
    }
}

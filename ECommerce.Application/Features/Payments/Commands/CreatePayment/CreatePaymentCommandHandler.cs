using ECommerce.Application.Abstractions.Repositories;
using ECommerce.Domain.Entities;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, int>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPaymentService _paymentService;

        public CreatePaymentCommandHandler(
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            IPaymentService paymentService

            )
        {
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _paymentService = paymentService;
        }

        public async Task<int> Handle(CreatePaymentCommand request,
            CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetByIdAsync(request.Id,cancellationToken);

            if (order is null)
            {
                throw new InvalidOperationException("Order not found.");
            }
            var userId = _currentUserService.UserId;

            if (userId is null)
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }
            if (order.UserId != userId.Value)
            {
                throw new UnauthorizedAccessException(
                    "You are not allowed to pay for this order.");
            }

            var existingPayment = await _paymentRepository.GetByIdAsync(request.Id,
                cancellationToken);

            if (existingPayment is not null)
            {
                throw new InvalidOperationException("Payment already exists for this order.");
            }

            var amount = order.GetTotal();

            var payment = Payment.Create(order.Id,amount);
            var paymentSucceeded = await _paymentService.ProcessPaymentAsync(amount,
                cancellationToken);

            if (!paymentSucceeded)
            {
                payment.MarkAsFailed();
            }
            else
            {
                payment.MarkAsPaid();
            }

            await _paymentRepository.AddAsync(payment,cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            return request.Id;
        }
    }
}

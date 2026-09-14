using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.Commands.MarkPaymentAsPaid
{
    public class MarkPaymentAsPaidCommandHandler : IRequestHandler<MarkPaymentAsPaidCommand>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MarkPaymentAsPaidCommandHandler(
            IPaymentRepository paymentRepository,
            IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(MarkPaymentAsPaidCommand request,
            CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetByIdAsync(request.PaymentId,
                cancellationToken);
            if (payment is null)
            {
                throw new KeyNotFoundException("Payment not found.");
            }
            payment.MarkAsPaid();
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

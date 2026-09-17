using ECommerce.Application.Abstractions.Services;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.IRepositories;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Application.Features.Payments.Commands.WebHook
{
    public class WebHookCommandHandler : IRequestHandler<WebHookCommand>
    {
        private readonly IPaymobWebhookService _paymobWebhookService;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        public WebHookCommandHandler(IPaymobWebhookService paymobWebhookService,
        IOrderRepository orderRepository, IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork,
        IProductRepository productRepository)
        {
            _paymobWebhookService = paymobWebhookService;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
            _productRepository = productRepository;
        }

        public async Task Handle(WebHookCommand request, CancellationToken cancellationToken)
        {
            var isValid = _paymobWebhookService.VerifyHmac(request.request);
            if (!isValid)
                throw new InvalidOperationException("Invalid HMAC.");
    


            var orderId = _paymobWebhookService.GetOrderId(request.request);
            var order = await _orderRepository.GetByIdAsync(orderId,cancellationToken);

            if (order is null)
                throw new KeyNotFoundException("Order not found.");

            var payment = await _paymentRepository.GetByOrderIdAsync(orderId, cancellationToken);
            if (payment is null)
                throw new KeyNotFoundException("Payment not found.");

            if (payment.Status != PaymentStatus.Pending)
                return;

            if (request.request.Obj.Success)
            {
                payment.MarkAsPaid();
                order.Confirm();
            }

            else
            {
                payment.MarkAsFailed();
                order.Cancel();
                var productIds = order.Items.Select(x => x.ProductId).Distinct().ToList();
                var products = await _productRepository.GetByIdsAsync(productIds,cancellationToken);
                var productsById = products.ToDictionary(x => x.Id);

                foreach (var item in order.Items)
                {
                    if (!productsById.TryGetValue(item.ProductId, out var product))
                        throw new KeyNotFoundException(
                            $"Product with id {item.ProductId} not found.");

                    product.IncreaseStock(item.Quantity);
                }
            }
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return;


        }
    }
}

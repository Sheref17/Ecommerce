using ECommerce.Application.Abstractions.Services;
using ECommerce.Application.Features.Orders.DTOs;
using ECommerce.Application.Features.Payments.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.Enums;
using ECommerce.Domain.Exceptions;
using ECommerce.Domain.IRepositories;
using MediatR;


namespace ECommerce.Application.Features.Orders.Commands.Checkout
{
    public class CheckoutCommandHandler : IRequestHandler<CheckoutCommand, CheckoutResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IBasketRepository _basketRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IPaymentService _paymentService;

        public CheckoutCommandHandler(
            ICurrentUserService currentUserService,
            IBasketRepository basketRepository,
            IProductRepository productRepository,
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            IPaymentService paymentService,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _basketRepository = basketRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _paymentService = paymentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<CheckoutResponse> Handle(CheckoutCommand request,
            CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                var userId = _currentUserService.UserId;
                var firstName = _currentUserService.FirstName;
                var lastName = _currentUserService.LastName;
                var email = _currentUserService.Email;


                if (userId is null)
                    throw new UnauthorizedAccessException("User is not authenticated.");
               
                var basket = await _basketRepository.GetByUserIdAsync(userId.Value,
                    cancellationToken);

                if (basket is null)
                    throw new KeyNotFoundException("Basket not found.");
              

                if (!basket.Items.Any())
                    throw new DomainException("Basket is empty.");

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
                var payment = Payment.Create(order.Id, order.GetTotal(), request.request.PaymentMethod);

                await _paymentRepository.AddAsync(payment, cancellationToken);

                string? paymentUrl = null;

                if (request.request.PaymentMethod == PaymentMethod.Visa)
                {
                    var paymentRequest = new PaymentRequest
                    {
                        Amount = order.GetTotal(),
                        Reference = order.Id.ToString(),
                        Items = order.Items.Select(item =>
                        {
                            var product = productsById[item.ProductId];

                            return new PaymentItem
                            {
                                Name = product.Name,
                                Amount = item.UnitPrice,
                                Description = product.Description,
                                Quantity = item.Quantity
                            };
                        }).ToList(),
                        BillingData = new BillingData
                        {
                            FirstName = firstName! ,
                            LastName = lastName! ,
                            Email = email!,
                            PhoneNumber = request.request.PhoneNumber,
                            Country = request.request.Country,
                            City = request.request.City,
                            Building =request.request.Building,
                            Floor = request.request.Floor,
                            Apartment = request.request.Apartment,
                            PostalCode = request.request.Apartment,
                            State = request.request.State,
                            Street = request.request.Street
                              
                        },
                    };

                    paymentUrl = await _paymentService.ProcessPaymentAsync(paymentRequest,
                        cancellationToken);
                }
                else
                    order.Confirm();
                

                basket.Clear();
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
                return new CheckoutResponse
                {
                    OrderId = order.Id,
                    PaymentId = payment.Id,
                    PaymentUrl = paymentUrl
                };

            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }
           
        }
    }
}

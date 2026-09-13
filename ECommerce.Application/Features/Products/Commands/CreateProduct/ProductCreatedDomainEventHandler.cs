using ECommerce.Domain.Events;
using ECommerce.Domain.Events.Products;
using MediatR;
using Microsoft.Extensions.Logging;

namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class ProductCreatedDomainEventHandler :
        INotificationHandler<ProductCreatedDomainEvent>
    {
        private readonly ILogger<ProductCreatedDomainEventHandler> _logger;
        public ProductCreatedDomainEventHandler(ILogger<ProductCreatedDomainEventHandler> logger)
        {
            _logger = logger;
            
        }

        public Task Handle(ProductCreatedDomainEvent notification
            , CancellationToken cancellationToken)
        {

            _logger.LogInformation("Product created successfully. ProductId: {ProductId}",
                notification.ProductId);
            return Task.CompletedTask;
        }
    }
}

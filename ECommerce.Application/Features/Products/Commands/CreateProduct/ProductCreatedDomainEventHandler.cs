using ECommerce.Application.Events;
using ECommerce.Domain.Events.Products;
using MediatR;
using Microsoft.Extensions.Logging;


namespace ECommerce.Application.Features.Products.Commands.CreateProduct
{
    public class ProductCreatedDomainEventHandler :
        IDomainEventHandler<ProductCreatedDomainEvent>
    {
        private readonly ILogger<ProductCreatedDomainEventHandler> _logger;
        public ProductCreatedDomainEventHandler(ILogger<ProductCreatedDomainEventHandler> logger)
        {
            _logger = logger;
            
        }

        public Task Handle(ProductCreatedDomainEvent domainEvent)
        {
            var product = domainEvent.Product;
            _logger.LogInformation("Product created successfully. ProductId: {ProductId}, " +
                "Name: {ProductName}",product.Id,product.Name);
                return Task.CompletedTask;
        }

     
    }
}

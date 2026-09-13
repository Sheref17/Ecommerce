using ECommerce.Domain.IRepositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.BackgroundServices
{
    public class DomainEventBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<DomainEventBackgroundService> _logger;
        private readonly IPublisher _publisher;

        public DomainEventBackgroundService(IServiceScopeFactory scopeFactory,
            ILogger<DomainEventBackgroundService> logger,
            IPublisher publisher
            )
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            _publisher = publisher;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var outboxRepository =scope.ServiceProvider
                    .GetRequiredService<IOutboxRepository>();
                var messages = await outboxRepository.GetUnprocessedAsync(20,stoppingToken);
                if (messages.Count == 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(5),stoppingToken);

                    continue;
                }
                foreach (var message in messages)
                {
                    try
                    {
                        var eventType = Type.GetType(message.Type);

                        if (eventType is null)
                        {
                            _logger.LogError("Could not resolve event type {EventType}"
                                , message.Type);

                            continue;
                        }
                        var domainEvent = JsonSerializer.Deserialize(message.Content, eventType);

                        if (domainEvent is not INotification notification)
                        {
                            _logger.LogError(
                                "Outbox message {MessageId} is not a valid notification",
                                message.Id);
                            continue;
                        }
                        await _publisher.Publish(notification, stoppingToken);
                        message.MarkAsProcessed();
                        await outboxRepository.UpdateAsync(message, stoppingToken);
                    }
                    catch(Exception ex) 
                    {
                        message.MarkAsFailed(ex.Message);

                        await outboxRepository.UpdateAsync(message,stoppingToken);
                    }
                }
                await scope.ServiceProvider.GetRequiredService<IUnitOfWork>()
                    .SaveChangesAsync();
                await Task.Delay(TimeSpan.FromSeconds(5),stoppingToken);
            }
        }
    }
}

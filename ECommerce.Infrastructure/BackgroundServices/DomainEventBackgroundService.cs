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
      

        public DomainEventBackgroundService(IServiceScopeFactory scopeFactory,
            ILogger<DomainEventBackgroundService> logger
            )
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
            
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();
                var outboxRepository =scope.ServiceProvider
                    .GetRequiredService<IOutboxRepository>();

                var publisher = scope.ServiceProvider
                    .GetRequiredService<IPublisher>();
                var messages = await outboxRepository.GetUnprocessedAsync(20,stoppingToken);
                if (messages.Count == 0)
                {
                    await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
                    continue;
                }
                foreach (var message in messages)
                {
                    try
                    {
                        var eventType = Type.GetType(message.Type);

                        if (eventType is null)
                        {
                            var error = $"Could not resolve event type {message.Type}";

                            _logger.LogError("Outbox message {MessageId}: {Error}",message.Id,error);

                            message.MarkAsFailed(error);
                            await outboxRepository.UpdateAsync(message, stoppingToken);

                            continue;
                        }
                        var domainEvent = JsonSerializer.Deserialize(message.Content, eventType);

                        if (domainEvent is not INotification notification)
                        {
                            var error = $"Outbox message {message.Id} is not a valid notification.";
                            _logger.LogError("{Error}", error);

                            message.MarkAsFailed(error);
                            await outboxRepository.UpdateAsync(message, stoppingToken);
                            continue;
                        }
                        await publisher.Publish(notification, stoppingToken);
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
                    .SaveChangesAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(5),stoppingToken);
            }
        }
    }
}

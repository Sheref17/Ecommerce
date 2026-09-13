using ECommerce.Domain.Common;
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Persistence.Interceptors
{
    public class OutboxSaveChangesInterceptor : SaveChangesInterceptor
    {
        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;

            if (context is null)
                return base.SavingChangesAsync(eventData,result,cancellationToken);

            var entities = context.ChangeTracker
                .Entries<BaseEntity>()
                .Where(x => x.Entity.DomainEvents.Any())
                .Select(x => x.Entity)
                .ToList();

            foreach (var entity in entities)
            {
                foreach (var domainEvent in entity.DomainEvents)
                {
                    var outboxMessage = new OutboxMessage(Guid.NewGuid(),
                        domainEvent.GetType().AssemblyQualifiedName!,
                        JsonSerializer.Serialize(domainEvent,domainEvent.GetType()),
                        domainEvent.OccurredOn);

                    context.Set<OutboxMessage>().Add(outboxMessage);
                }
            }

            return base.SavingChangesAsync(eventData,result,cancellationToken);
        }

        public override ValueTask<int> SavedChangesAsync(
            SaveChangesCompletedEventData eventData,int result,
            CancellationToken cancellationToken = default)
        {
            var context = eventData.Context;

            if (context is not null)
            {
                var entities = context.ChangeTracker
                    .Entries<BaseEntity>()
                    .Where(x => x.Entity.DomainEvents.Any())
                    .Select(x => x.Entity)
                    .ToList();

                foreach (var entity in entities)
                {
                    entity.ClearDomainEvents();
                }
            }

            return base.SavedChangesAsync(eventData,result,cancellationToken);
        }
    }
}

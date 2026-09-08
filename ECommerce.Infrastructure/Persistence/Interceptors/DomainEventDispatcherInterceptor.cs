using ECommerce.Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ECommerce.Infrastructure.Persistence.Interceptors;


public class DomainEventDispatcherInterceptor
    : SaveChangesInterceptor
{
    private readonly DomainEventDispatcher _dispatcher;

    public DomainEventDispatcherInterceptor(
        DomainEventDispatcher dispatcher)
    {
        _dispatcher = dispatcher;
    }

    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        if (context is null)
            return result;

        var entities = context.ChangeTracker
            .Entries<BaseEntity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .Select(x => x.Entity)
            .ToList();

        var domainEvents = entities
            .SelectMany(x => x.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await _dispatcher.DispatchAsync(domainEvent,cancellationToken);
        }

        foreach (var entity in entities)
        {
            entity.ClearDomainEvents();
        }

        return result;
    }
}
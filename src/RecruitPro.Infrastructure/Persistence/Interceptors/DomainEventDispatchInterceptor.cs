using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecruitPro.Application.Common.Models;
using RecruitPro.Domain.Common;

namespace RecruitPro.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Publishes BaseEntity.DomainEvents through MediatR after the transaction actually commits
/// (SavedChangesAsync, not SavingChangesAsync), wrapping each event in DomainEventNotification&lt;T&gt;
/// via reflection so Domain stays free of a MediatR dependency. Events are cleared from their
/// entities before publishing (not after), so if a handler performs its own SaveChangesAsync call
/// against this same DbContext instance, that nested save doesn't re-dispatch the same events.
/// </summary>
public sealed class DomainEventDispatchInterceptor(IPublisher publisher) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        await DispatchDomainEventsAsync(eventData.Context, cancellationToken);
        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEventsAsync(DbContext? context, CancellationToken cancellationToken)
    {
        if (context is null) return;

        var entitiesWithEvents = context.ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Count > 0)
            .ToList();

        var domainEvents = entitiesWithEvents.SelectMany(e => e.DomainEvents).ToList();
        foreach (var entity in entitiesWithEvents)
            entity.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            var wrapperType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = (INotification)Activator.CreateInstance(wrapperType, domainEvent)!;
            await publisher.Publish(notification, cancellationToken);
        }
    }
}

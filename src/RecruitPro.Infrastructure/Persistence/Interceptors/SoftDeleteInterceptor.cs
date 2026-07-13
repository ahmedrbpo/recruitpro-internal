using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Domain.Common;

namespace RecruitPro.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Converts every DbContext.Remove() into IsDeleted = true (plus DeletedAt/DeletedBy) so
/// application code never has to remember to soft-delete manually and a hard DELETE is not
/// reachable through the ORM. Must run before AuditableEntitySaveChangesInterceptor so the audit
/// log sees the converted Modified state rather than a Deleted one.
/// </summary>
public sealed class SoftDeleteInterceptor(ICurrentUserService currentUserService, IDateTimeProvider dateTimeProvider)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        ConvertDeletesToSoftDeletes(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        ConvertDeletesToSoftDeletes(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void ConvertDeletesToSoftDeletes(DbContext? context)
    {
        if (context is null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State != EntityState.Deleted) continue;

            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
            entry.Entity.DeletedAt = dateTimeProvider.UtcNow;
            entry.Entity.DeletedBy = currentUserService.UserId;
        }
    }
}

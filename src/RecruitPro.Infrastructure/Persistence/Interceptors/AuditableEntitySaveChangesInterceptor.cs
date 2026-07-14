using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Domain.Common;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Infrastructure.Persistence.Interceptors;

/// <summary>
/// Stamps CreatedAt/By and ModifiedAt/By automatically, assigns a fresh RowVersion on every
/// write (Postgres bytea columns don't auto-increment the way SQL Server ROWVERSION does, so the
/// app must generate the new concurrency-token value itself), and writes an AuditLog row per
/// entity change with a before/after diff — all without per-handler boilerplate.
/// </summary>
public sealed class AuditableEntitySaveChangesInterceptor(
    ICurrentUserService currentUserService,
    IDateTimeProvider dateTimeProvider)
    : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null) return;

        var now = dateTimeProvider.UtcNow;
        var userId = currentUserService.UserId;
        var auditLogs = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State is not (EntityState.Added or EntityState.Modified))
                continue;

            entry.Entity.RowVersion = Guid.NewGuid().ToByteArray();

            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = now;
                entry.Entity.CreatedBy = userId;
                auditLogs.Add(BuildAuditLog(entry, AuditAction.Create, SerializeCurrentValues(entry), now, userId));
                continue;
            }

            entry.Entity.ModifiedAt = now;
            entry.Entity.ModifiedBy = userId;

            // SoftDeleteInterceptor turns a Remove() into IsDeleted flipping true here — log
            // that as Delete, not Update.
            var isSoftDelete = entry.Property(nameof(BaseEntity.IsDeleted)).IsModified && entry.Entity.IsDeleted;
            auditLogs.Add(BuildAuditLog(entry, isSoftDelete ? AuditAction.Delete : AuditAction.Update, SerializeChangedProperties(entry), now, userId));
        }

        foreach (var log in auditLogs)
            context.Set<AuditLog>().Add(log);
    }

    private static AuditLog BuildAuditLog(EntityEntry<BaseEntity> entry, AuditAction action, string changes, DateTimeOffset now, Guid? userId) =>
        AuditLog.Create(entry.Entity.GetType().Name, entry.Entity.Id, action, changes, userId, now);

    private static string SerializeChangedProperties(EntityEntry<BaseEntity> entry)
    {
        var diff = entry.Properties
            .Where(p => p.IsModified)
            .ToDictionary(p => p.Metadata.Name, p => new { Old = Redact(p.Metadata.Name, p.OriginalValue), New = Redact(p.Metadata.Name, p.CurrentValue) });

        return JsonSerializer.Serialize(diff);
    }

    private static string SerializeCurrentValues(EntityEntry<BaseEntity> entry)
    {
        var values = entry.Properties.ToDictionary(p => p.Metadata.Name, p => Redact(p.Metadata.Name, p.CurrentValue));
        return JsonSerializer.Serialize(values);
    }

    /// <summary>Audit rows are meant to be broadly readable (an AuditLogsController now exposes
    /// them via the API), so secret-bearing columns must never round-trip into the Changes diff
    /// even in hashed form — a password/token hash doesn't belong in a general-purpose audit
    /// viewer just because the entity that owns it happened to change. Matches by suffix so any
    /// future *Hash-named column (PasswordHash, TokenHash, ReplacedByTokenHash, ...) is covered
    /// without needing to enumerate every entity here.</summary>
    private static object? Redact(string propertyName, object? value) =>
        propertyName.EndsWith("Hash", StringComparison.Ordinal) && value is not null ? "[REDACTED]" : value;
}

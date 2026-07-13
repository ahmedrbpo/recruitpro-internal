namespace RecruitPro.Domain.Identity.Entities;

/// <summary>
/// Immutable compliance-grade record of a command execution, written automatically from the
/// AuditableEntitySaveChangesInterceptor. Deliberately does not inherit BaseEntity: audit rows
/// are never modified or soft-deleted.
/// </summary>
public sealed class AuditLog
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public string EntityType { get; private set; } = default!;
    public Guid EntityId { get; private set; }
    public AuditAction Action { get; private set; }
    public string? Changes { get; private set; }
    public Guid? UserId { get; private set; }
    public DateTimeOffset Timestamp { get; private set; }

    private AuditLog() { } // EF Core

    public static AuditLog Create(string entityType, Guid entityId, AuditAction action, string? changes, Guid? userId, DateTimeOffset timestamp) =>
        new()
        {
            EntityType = entityType,
            EntityId = entityId,
            Action = action,
            Changes = changes,
            UserId = userId,
            Timestamp = timestamp,
        };
}

using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Application.Identity.Dtos;

public sealed record AuditLogDto(
    Guid Id,
    string EntityType,
    Guid EntityId,
    AuditAction Action,
    string? Changes,
    Guid? UserId,
    DateTimeOffset Timestamp)
{
    public static AuditLogDto FromEntity(AuditLog log) =>
        new(log.Id, log.EntityType, log.EntityId, log.Action, log.Changes, log.UserId, log.Timestamp);
}

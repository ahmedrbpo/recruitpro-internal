using RecruitPro.Domain.Notifications.Entities;

namespace RecruitPro.Application.Notifications.Dtos;

public sealed record NotificationLogDto(
    Guid Id,
    NotificationChannel Channel,
    string RecipientEmail,
    string? RecipientName,
    string? TemplateCode,
    string Subject,
    NotificationStatus Status,
    int Attempts,
    string? LastError,
    DateTimeOffset? SentAt,
    Guid? RelatedEntityId,
    string? RelatedEntityType,
    DateTimeOffset CreatedAt)
{
    public static NotificationLogDto FromEntity(NotificationLog log) =>
        new(log.Id, log.Channel, log.RecipientEmail, log.RecipientName, log.TemplateCode, log.Subject, log.Status,
            log.Attempts, log.LastError, log.SentAt, log.RelatedEntityId, log.RelatedEntityType, log.CreatedAt);
}

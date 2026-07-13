using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Notifications.Entities;

/// <summary>Doubles as the outbound send queue and the delivery history: rows are created
/// Pending by a lifecycle-event handler at the moment a notification is warranted, then picked
/// up and transitioned to Sent/Failed by the background send job. RelatedEntityId/Type mirror
/// AuditLog's polymorphic-reference convention, so a notification can be traced back to the
/// job/application/interview/offer that triggered it.</summary>
public sealed class NotificationLog : BaseEntity
{
    public NotificationChannel Channel { get; private set; }
    public string RecipientEmail { get; private set; } = default!;
    public string? RecipientName { get; private set; }
    public string? TemplateCode { get; private set; }
    public string Subject { get; private set; } = default!;
    public string Body { get; private set; } = default!;
    public NotificationStatus Status { get; private set; } = NotificationStatus.Pending;
    public int Attempts { get; private set; }
    public string? LastError { get; private set; }
    public DateTimeOffset? SentAt { get; private set; }
    public Guid? RelatedEntityId { get; private set; }
    public string? RelatedEntityType { get; private set; }

    private NotificationLog() { } // EF Core

    public static NotificationLog Create(
        NotificationChannel channel,
        string recipientEmail,
        string? recipientName,
        string? templateCode,
        string subject,
        string body,
        Guid? relatedEntityId,
        string? relatedEntityType) =>
        new()
        {
            Channel = channel,
            RecipientEmail = recipientEmail,
            RecipientName = recipientName,
            TemplateCode = templateCode,
            Subject = subject,
            Body = body,
            RelatedEntityId = relatedEntityId,
            RelatedEntityType = relatedEntityType,
        };

    public void MarkSent(DateTimeOffset sentAt)
    {
        Attempts++;
        Status = NotificationStatus.Sent;
        SentAt = sentAt;
        LastError = null;
    }

    public void MarkFailed(string error)
    {
        Attempts++;
        Status = NotificationStatus.Failed;
        LastError = error;
    }
}

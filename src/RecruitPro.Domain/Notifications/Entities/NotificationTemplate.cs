using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Notifications.Entities;

/// <summary>A reusable message template keyed by a unique Code (e.g. "APPLICATION_INTERVIEW",
/// "INTERVIEW_SCHEDULED"). Subject and Body support {{Token}} placeholders substituted at send
/// time. Lifecycle-event handlers look up a template by code and silently skip sending if none
/// is found or the template is inactive, so a missing template never breaks the triggering
/// workflow (publishing a job, moving an application stage, etc.).</summary>
public sealed class NotificationTemplate : BaseEntity
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public NotificationChannel Channel { get; private set; }
    public string Subject { get; private set; } = default!;
    public string Body { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    private NotificationTemplate() { } // EF Core

    public static NotificationTemplate Create(string code, string name, NotificationChannel channel, string subject, string body) =>
        new()
        {
            Code = code.Trim().ToUpperInvariant(),
            Name = name,
            Channel = channel,
            Subject = subject,
            Body = body,
        };

    public void Deactivate() => IsActive = false;

    public void Activate() => IsActive = true;
}

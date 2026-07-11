using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Entities;

public sealed class ApplicationStageHistory : BaseEntity
{
    public Guid ApplicationId { get; private set; }
    public JobApplication? Application { get; private set; }
    public string FromStage { get; private set; } = default!;
    public string ToStage { get; private set; } = default!;
    public DateTimeOffset ChangedAt { get; private set; }
    public Guid? ChangedBy { get; private set; }

    private ApplicationStageHistory() { } // EF Core

    public static ApplicationStageHistory Create(Guid applicationId, string fromStage, string toStage, DateTimeOffset changedAt, Guid? changedBy) =>
        new()
        {
            ApplicationId = applicationId,
            FromStage = fromStage,
            ToStage = toStage,
            ChangedAt = changedAt,
            ChangedBy = changedBy,
        };
}

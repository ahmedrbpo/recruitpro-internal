using RecruitPro.Domain.Common;
using RecruitPro.Domain.Recruitment.Entities;

namespace RecruitPro.Domain.Recruitment.Events;

public sealed record InterviewScheduledEvent(
    Guid InterviewId,
    Guid ApplicationId,
    DateTimeOffset ScheduledAt,
    InterviewMode Mode,
    int Round) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}

using RecruitPro.Domain.Common;

namespace RecruitPro.Domain.Recruitment.Events;

/// <summary>Raised both when a JobApplication is first created (PreviousStage null, NewStage
/// Applied) and on every subsequent MoveToStage call, so a single notification handler can cover
/// the whole pipeline (applied/screening/interview/offer/hired/rejected) rather than one event
/// type per stage.</summary>
public sealed record ApplicationStageChangedEvent(
    Guid ApplicationId,
    Guid CandidateId,
    Guid JobId,
    string? PreviousStage,
    string NewStage) : IDomainEvent
{
    public DateTimeOffset OccurredOn { get; } = DateTimeOffset.UtcNow;
}

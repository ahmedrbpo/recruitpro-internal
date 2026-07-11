using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.ValueObjects;

namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>
/// Named JobApplication rather than the blueprint's bare "Application": within the
/// RecruitPro.Application project, an unqualified "Application" identifier resolves to the
/// RecruitPro.Application namespace itself (a parent-namespace lookup, not a using-directive
/// one), which would produce a CS0118 "is a namespace but is used like a type" error on every
/// reference. The database table is still named "applications" per the blueprint's schema.
/// </summary>
public sealed class JobApplication : BaseEntity
{
    public Guid JobId { get; private set; }
    public Job? Job { get; private set; }

    public Guid CandidateId { get; private set; }
    public Candidate? Candidate { get; private set; }

    public string Stage { get; private set; } = ApplicationStage.Applied;

    private readonly List<ApplicationStageHistory> _stageHistory = [];
    public IReadOnlyCollection<ApplicationStageHistory> StageHistory => _stageHistory.AsReadOnly();

    private JobApplication() { } // EF Core

    public static JobApplication Create(Guid jobId, Guid candidateId) =>
        new() { JobId = jobId, CandidateId = candidateId };

    /// <summary>Enforces the fixed pipeline shape: rejects transitions that skip a stage,
    /// reverse the pipeline, or move out of a terminal stage.</summary>
    public void MoveToStage(string newStage, DateTimeOffset now, Guid? changedBy)
    {
        if (ApplicationStage.IsTerminal(Stage))
            throw new ApplicationStageTransitionException(Stage, newStage, "the application is already in a terminal stage");

        if (!ApplicationStage.IsValidTransition(Stage, newStage))
            throw new ApplicationStageTransitionException(Stage, newStage, "that transition skips a stage or reverses the pipeline");

        _stageHistory.Add(ApplicationStageHistory.Create(Id, Stage, newStage, now, changedBy));
        Stage = newStage;
    }
}

using RecruitPro.Domain.Common;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Domain.Recruitment.Entities;

/// <summary>One scheduled round of an application's interview process. Named Interview rather
/// than colliding with anything — no namespace ambiguity risk here, unlike JobApplication.</summary>
public sealed class Interview : BaseEntity
{
    public Guid ApplicationId { get; private set; }
    public JobApplication? Application { get; private set; }
    public DateTimeOffset ScheduledAt { get; private set; }
    public int? DurationMinutes { get; private set; }
    public InterviewMode Mode { get; private set; }
    public int Round { get; private set; }
    public Guid? InterviewerId { get; private set; }
    public ApplicationUser? Interviewer { get; private set; }
    public InterviewStatus Status { get; private set; } = InterviewStatus.Scheduled;
    public string? Notes { get; private set; }

    private readonly List<InterviewFeedback> _feedback = [];
    public IReadOnlyCollection<InterviewFeedback> Feedback => _feedback.AsReadOnly();

    private Interview() { } // EF Core

    public static Interview Schedule(
        Guid applicationId,
        DateTimeOffset scheduledAt,
        InterviewMode mode,
        int round,
        Guid? interviewerId = null,
        int? durationMinutes = null,
        string? notes = null) =>
        new()
        {
            ApplicationId = applicationId,
            ScheduledAt = scheduledAt,
            Mode = mode,
            Round = round,
            InterviewerId = interviewerId,
            DurationMinutes = durationMinutes,
            Notes = notes,
        };

    public void Reschedule(DateTimeOffset newScheduledAt)
    {
        if (Status != InterviewStatus.Scheduled)
            throw new InterviewStateTransitionException("reschedule", $"it is {Status}, not Scheduled");

        ScheduledAt = newScheduledAt;
    }

    public void Complete()
    {
        if (Status != InterviewStatus.Scheduled)
            throw new InterviewStateTransitionException("complete", $"it is {Status}, not Scheduled");

        Status = InterviewStatus.Completed;
    }

    public void Cancel()
    {
        if (Status != InterviewStatus.Scheduled)
            throw new InterviewStateTransitionException("cancel", $"it is {Status}, not Scheduled");

        Status = InterviewStatus.Cancelled;
    }

    /// <summary>Feedback can only be recorded once the interview has actually happened.</summary>
    public void RecordFeedback(Guid interviewerId, int rating, RecommendationType recommendation, string? comments)
    {
        if (Status != InterviewStatus.Completed)
            throw new InterviewStateTransitionException("record feedback for", $"it is {Status}, not Completed");

        _feedback.Add(InterviewFeedback.Create(Id, interviewerId, rating, recommendation, comments));
    }
}

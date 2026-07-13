using FluentAssertions;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using Xunit;

namespace RecruitPro.Domain.Tests.Recruitment;

public sealed class InterviewTests
{
    private static readonly DateTimeOffset ScheduledAt = new(2026, 1, 10, 10, 0, 0, TimeSpan.Zero);

    private static Interview Schedule() => Interview.Schedule(Guid.NewGuid(), ScheduledAt, InterviewMode.Video, round: 1);

    [Fact]
    public void Reschedule_WhileScheduled_UpdatesScheduledAt()
    {
        var interview = Schedule();
        var newTime = ScheduledAt.AddDays(1);

        interview.Reschedule(newTime);

        interview.ScheduledAt.Should().Be(newTime);
    }

    [Fact]
    public void Complete_WhileScheduled_Succeeds()
    {
        var interview = Schedule();

        interview.Complete();

        interview.Status.Should().Be(InterviewStatus.Completed);
    }

    [Fact]
    public void Complete_AlreadyCompleted_ThrowsInterviewStateTransitionException()
    {
        var interview = Schedule();
        interview.Complete();

        var act = interview.Complete;

        act.Should().Throw<InterviewStateTransitionException>();
    }

    [Fact]
    public void Cancel_WhileScheduled_Succeeds()
    {
        var interview = Schedule();

        interview.Cancel();

        interview.Status.Should().Be(InterviewStatus.Cancelled);
    }

    [Fact]
    public void RecordFeedback_BeforeCompleted_ThrowsInterviewStateTransitionException()
    {
        var interview = Schedule();

        var act = () => interview.RecordFeedback(Guid.NewGuid(), 4, RecommendationType.Hire, "Solid candidate.");

        act.Should().Throw<InterviewStateTransitionException>();
    }

    [Fact]
    public void RecordFeedback_AfterCompleted_Succeeds()
    {
        var interview = Schedule();
        interview.Complete();

        interview.RecordFeedback(Guid.NewGuid(), 4, RecommendationType.Hire, "Solid candidate.");

        interview.Feedback.Should().ContainSingle(f => f.Rating == 4 && f.Recommendation == RecommendationType.Hire);
    }

    [Fact]
    public void RecordFeedback_RatingOutOfRange_ThrowsInvalidInterviewRatingException()
    {
        var interview = Schedule();
        interview.Complete();

        var act = () => interview.RecordFeedback(Guid.NewGuid(), 6, RecommendationType.Hire, null);

        act.Should().Throw<InvalidInterviewRatingException>();
    }
}

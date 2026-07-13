using FluentAssertions;
using RecruitPro.Domain.Common.Exceptions;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.Events;
using RecruitPro.Domain.Recruitment.ValueObjects;
using Xunit;

namespace RecruitPro.Domain.Tests.Recruitment;

public sealed class JobApplicationTests
{
    private static readonly DateTimeOffset Now = DateTimeOffset.UtcNow;

    [Fact]
    public void Create_RaisesApplicationStageChangedEventWithNullPreviousStage()
    {
        var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid());

        var domainEvent = Assert.Single(application.DomainEvents) as ApplicationStageChangedEvent;

        domainEvent.Should().NotBeNull();
        domainEvent!.PreviousStage.Should().BeNull();
        domainEvent.NewStage.Should().Be(ApplicationStage.Applied);
    }

    [Fact]
    public void MoveToStage_RaisesApplicationStageChangedEventWithPreviousAndNewStage()
    {
        var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid());
        application.ClearDomainEvents();

        application.MoveToStage(ApplicationStage.Screening, Now, changedBy: null);

        var domainEvent = Assert.Single(application.DomainEvents) as ApplicationStageChangedEvent;

        domainEvent.Should().NotBeNull();
        domainEvent!.PreviousStage.Should().Be(ApplicationStage.Applied);
        domainEvent.NewStage.Should().Be(ApplicationStage.Screening);
    }

    [Theory]
    [InlineData(ApplicationStage.Applied, ApplicationStage.Screening)]
    [InlineData(ApplicationStage.Screening, ApplicationStage.Interview)]
    [InlineData(ApplicationStage.Interview, ApplicationStage.Offer)]
    [InlineData(ApplicationStage.Offer, ApplicationStage.Hired)]
    public void MoveToStage_ValidForwardTransition_Succeeds(string from, string to)
    {
        var application = CreateApplicationAt(from);

        application.MoveToStage(to, Now, changedBy: null);

        application.Stage.Should().Be(to);
    }

    [Theory]
    [InlineData(ApplicationStage.Applied)]
    [InlineData(ApplicationStage.Screening)]
    [InlineData(ApplicationStage.Interview)]
    [InlineData(ApplicationStage.Offer)]
    public void MoveToStage_RejectedFromAnyNonTerminalStage_Succeeds(string from)
    {
        var application = CreateApplicationAt(from);

        application.MoveToStage(ApplicationStage.Rejected, Now, changedBy: null);

        application.Stage.Should().Be(ApplicationStage.Rejected);
    }

    [Fact]
    public void MoveToStage_SkippingAStage_ThrowsApplicationStageTransitionException()
    {
        var application = CreateApplicationAt(ApplicationStage.Applied);

        var act = () => application.MoveToStage(ApplicationStage.Interview, Now, changedBy: null);

        act.Should().Throw<ApplicationStageTransitionException>();
    }

    [Fact]
    public void MoveToStage_HiredIsUnreachableExceptFromOffer()
    {
        var application = CreateApplicationAt(ApplicationStage.Interview);

        var act = () => application.MoveToStage(ApplicationStage.Hired, Now, changedBy: null);

        act.Should().Throw<ApplicationStageTransitionException>();
    }

    [Theory]
    [InlineData(ApplicationStage.Hired)]
    [InlineData(ApplicationStage.Rejected)]
    public void MoveToStage_FromTerminalStage_ThrowsApplicationStageTransitionException(string terminalStage)
    {
        var application = CreateApplicationAt(terminalStage);

        var act = () => application.MoveToStage(ApplicationStage.Screening, Now, changedBy: null);

        act.Should().Throw<ApplicationStageTransitionException>();
    }

    [Fact]
    public void MoveToStage_RecordsStageHistory()
    {
        var application = CreateApplicationAt(ApplicationStage.Applied);

        application.MoveToStage(ApplicationStage.Screening, Now, changedBy: null);

        application.StageHistory.Should().ContainSingle(h =>
            h.FromStage == ApplicationStage.Applied && h.ToStage == ApplicationStage.Screening);
    }

    private static JobApplication CreateApplicationAt(string stage)
    {
        var application = JobApplication.Create(Guid.NewGuid(), Guid.NewGuid());

        // Walk the application to the target stage through valid transitions so tests exercise
        // the real state machine rather than reaching into private state.
        var path = stage switch
        {
            ApplicationStage.Applied => Array.Empty<string>(),
            ApplicationStage.Screening => [ApplicationStage.Screening],
            ApplicationStage.Interview => [ApplicationStage.Screening, ApplicationStage.Interview],
            ApplicationStage.Offer => [ApplicationStage.Screening, ApplicationStage.Interview, ApplicationStage.Offer],
            ApplicationStage.Hired =>
                [ApplicationStage.Screening, ApplicationStage.Interview, ApplicationStage.Offer, ApplicationStage.Hired],
            ApplicationStage.Rejected => [ApplicationStage.Rejected],
            _ => throw new ArgumentOutOfRangeException(nameof(stage)),
        };

        foreach (var next in path)
            application.MoveToStage(next, Now, changedBy: null);

        return application;
    }
}

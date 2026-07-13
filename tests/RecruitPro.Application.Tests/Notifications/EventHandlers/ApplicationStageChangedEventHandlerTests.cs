using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.EventHandlers;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Notifications.Entities;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.Events;
using Xunit;

namespace RecruitPro.Application.Tests.Notifications.EventHandlers;

public sealed class ApplicationStageChangedEventHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private ApplicationStageChangedEventHandler CreateHandler() => new(_db);

    private async Task<(Candidate Candidate, Job Job)> SeedCandidateAndJobAsync()
    {
        var candidate = Candidate.Create("Ada", "Lovelace", "ada@example.com");
        var job = Job.Create("RP-2026-000001", "Senior .NET Developer", "desc", EmploymentType.FullTime, WorkMode.Remote,
            2, 5, "INR");
        _db.Candidates.Add(candidate);
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync(CancellationToken.None);
        return (candidate, job);
    }

    [Fact]
    public async Task Handle_ActiveTemplateExists_QueuesRenderedNotificationLog()
    {
        var (candidate, job) = await SeedCandidateAndJobAsync();
        var template = NotificationTemplate.Create(
            "APPLICATION_SCREENING", "Screening notice", NotificationChannel.Email,
            "You're in screening for {{JobTitle}}", "Hi {{CandidateName}}, you've moved to {{Stage}} for {{JobTitle}}.");
        _db.NotificationTemplates.Add(template);
        await _db.SaveChangesAsync(CancellationToken.None);

        var domainEvent = new ApplicationStageChangedEvent(Guid.NewGuid(), candidate.Id, job.Id, "applied", "screening");

        await CreateHandler().Handle(new DomainEventNotification<ApplicationStageChangedEvent>(domainEvent), CancellationToken.None);

        var log = Assert.Single(_db.NotificationLogs);
        log.RecipientEmail.Should().Be("ada@example.com");
        log.TemplateCode.Should().Be("APPLICATION_SCREENING");
        log.Subject.Should().Be("You're in screening for Senior .NET Developer");
        log.Body.Should().Be("Hi Ada Lovelace, you've moved to screening for Senior .NET Developer.");
        log.Status.Should().Be(NotificationStatus.Pending);
        log.RelatedEntityType.Should().Be(nameof(JobApplication));
    }

    [Fact]
    public async Task Handle_NoTemplateForStage_QueuesNothing()
    {
        var (candidate, job) = await SeedCandidateAndJobAsync();
        var domainEvent = new ApplicationStageChangedEvent(Guid.NewGuid(), candidate.Id, job.Id, "applied", "screening");

        await CreateHandler().Handle(new DomainEventNotification<ApplicationStageChangedEvent>(domainEvent), CancellationToken.None);

        _db.NotificationLogs.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_TemplateInactive_QueuesNothing()
    {
        var (candidate, job) = await SeedCandidateAndJobAsync();
        var template = NotificationTemplate.Create(
            "APPLICATION_SCREENING", "Screening notice", NotificationChannel.Email, "Subject", "Body");
        template.Deactivate();
        _db.NotificationTemplates.Add(template);
        await _db.SaveChangesAsync(CancellationToken.None);

        var domainEvent = new ApplicationStageChangedEvent(Guid.NewGuid(), candidate.Id, job.Id, "applied", "screening");

        await CreateHandler().Handle(new DomainEventNotification<ApplicationStageChangedEvent>(domainEvent), CancellationToken.None);

        _db.NotificationLogs.Should().BeEmpty();
    }
}

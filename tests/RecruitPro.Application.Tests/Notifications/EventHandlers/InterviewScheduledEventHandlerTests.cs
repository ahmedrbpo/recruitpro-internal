using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.EventHandlers;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Notifications.Entities;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.Events;
using Xunit;

namespace RecruitPro.Application.Tests.Notifications.EventHandlers;

public sealed class InterviewScheduledEventHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private InterviewScheduledEventHandler CreateHandler() => new(_db);

    [Fact]
    public async Task Handle_ActiveTemplateExists_QueuesRenderedNotificationLog()
    {
        var candidate = Candidate.Create("Grace", "Hopper", "grace@example.com");
        var job = Job.Create("RP-2026-000002", "Backend Engineer", "desc", EmploymentType.FullTime, WorkMode.Onsite, 1, 3, "INR");
        _db.Candidates.Add(candidate);
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync(CancellationToken.None);

        var application = JobApplication.Create(job.Id, candidate.Id);
        _db.Applications.Add(application);

        var template = NotificationTemplate.Create(
            "INTERVIEW_SCHEDULED", "Interview scheduled", NotificationChannel.Email,
            "Interview for {{JobTitle}}", "Hi {{CandidateName}}, round {{Round}} is scheduled.");
        _db.NotificationTemplates.Add(template);
        await _db.SaveChangesAsync(CancellationToken.None);

        var scheduledAt = new DateTimeOffset(2026, 8, 1, 10, 0, 0, TimeSpan.Zero);
        var domainEvent = new InterviewScheduledEvent(Guid.NewGuid(), application.Id, scheduledAt, InterviewMode.Video, 1);

        await CreateHandler().Handle(new DomainEventNotification<InterviewScheduledEvent>(domainEvent), CancellationToken.None);

        var log = Assert.Single(_db.NotificationLogs);
        log.RecipientEmail.Should().Be("grace@example.com");
        log.TemplateCode.Should().Be("INTERVIEW_SCHEDULED");
        log.Subject.Should().Be("Interview for Backend Engineer");
        log.Body.Should().Be("Hi Grace Hopper, round 1 is scheduled.");
        log.RelatedEntityType.Should().Be(nameof(Interview));
    }

    [Fact]
    public async Task Handle_NoTemplate_QueuesNothing()
    {
        var domainEvent = new InterviewScheduledEvent(Guid.NewGuid(), Guid.NewGuid(), DateTimeOffset.UtcNow, InterviewMode.Video, 1);

        await CreateHandler().Handle(new DomainEventNotification<InterviewScheduledEvent>(domainEvent), CancellationToken.None);

        _db.NotificationLogs.Should().BeEmpty();
    }
}

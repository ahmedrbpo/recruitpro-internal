using FluentAssertions;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.EventHandlers;
using RecruitPro.Application.Tests.Common;
using RecruitPro.Domain.Notifications.Entities;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.Events;
using Xunit;

namespace RecruitPro.Application.Tests.Notifications.EventHandlers;

public sealed class OfferExtendedEventHandlerTests
{
    private readonly TestApplicationDbContext _db = TestApplicationDbContext.CreateInMemory();

    private OfferExtendedEventHandler CreateHandler() => new(_db);

    [Fact]
    public async Task Handle_ActiveTemplateExists_QueuesRenderedNotificationLog()
    {
        var candidate = Candidate.Create("Alan", "Turing", "alan@example.com");
        var job = Job.Create("RP-2026-000003", "Principal Engineer", "desc", EmploymentType.FullTime, WorkMode.Hybrid, 5, 10, "INR");
        _db.Candidates.Add(candidate);
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync(CancellationToken.None);

        var application = JobApplication.Create(job.Id, candidate.Id);
        _db.Applications.Add(application);

        var template = NotificationTemplate.Create(
            "OFFER_EXTENDED", "Offer extended", NotificationChannel.Email,
            "Your offer for {{JobTitle}}", "Hi {{CandidateName}}, we're offering {{OfferedSalary}} {{CurrencyCode}}.");
        _db.NotificationTemplates.Add(template);
        await _db.SaveChangesAsync(CancellationToken.None);

        var domainEvent = new OfferExtendedEvent(Guid.NewGuid(), application.Id, 3_000_000m, "INR");

        await CreateHandler().Handle(new DomainEventNotification<OfferExtendedEvent>(domainEvent), CancellationToken.None);

        var log = Assert.Single(_db.NotificationLogs);
        log.RecipientEmail.Should().Be("alan@example.com");
        log.TemplateCode.Should().Be("OFFER_EXTENDED");
        log.Subject.Should().Be("Your offer for Principal Engineer");
        log.Body.Should().Be("Hi Alan Turing, we're offering 3,000,000.00 INR.");
        log.RelatedEntityType.Should().Be(nameof(Offer));
    }

    [Fact]
    public async Task Handle_NoTemplate_QueuesNothing()
    {
        var domainEvent = new OfferExtendedEvent(Guid.NewGuid(), Guid.NewGuid(), 1_000_000m, "INR");

        await CreateHandler().Handle(new DomainEventNotification<OfferExtendedEvent>(domainEvent), CancellationToken.None);

        _db.NotificationLogs.Should().BeEmpty();
    }
}

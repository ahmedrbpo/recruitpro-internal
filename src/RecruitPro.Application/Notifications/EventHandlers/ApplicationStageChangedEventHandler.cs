using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.Common;
using RecruitPro.Domain.Notifications.Entities;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.Events;

namespace RecruitPro.Application.Notifications.EventHandlers;

/// <summary>Covers every stage a JobApplication passes through (including the initial "applied"
/// stage set at creation) with a single handler: the template code is derived from the stage
/// name, e.g. "APPLICATION_INTERVIEW". If no active template exists for a stage, the handler
/// silently does nothing — a missing template must never break the pipeline transition itself.</summary>
public sealed class ApplicationStageChangedEventHandler(IApplicationDbContext db)
    : INotificationHandler<DomainEventNotification<ApplicationStageChangedEvent>>
{
    public async Task Handle(DomainEventNotification<ApplicationStageChangedEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        var templateCode = $"APPLICATION_{domainEvent.NewStage}".ToUpperInvariant();

        var template = await db.NotificationTemplates.AsNoTracking()
            .SingleOrDefaultAsync(t => t.Code == templateCode && t.IsActive, cancellationToken);
        if (template is null) return;

        var candidate = await db.Candidates.AsNoTracking()
            .SingleOrDefaultAsync(c => c.Id == domainEvent.CandidateId, cancellationToken);
        var job = await db.Jobs.AsNoTracking()
            .SingleOrDefaultAsync(j => j.Id == domainEvent.JobId, cancellationToken);
        if (candidate is null || job is null) return;

        var tokens = new Dictionary<string, string>
        {
            ["CandidateName"] = $"{candidate.FirstName} {candidate.LastName}",
            ["JobTitle"] = job.Title,
            ["Stage"] = domainEvent.NewStage,
        };

        var log = NotificationLog.Create(
            NotificationChannel.Email,
            candidate.Email,
            $"{candidate.FirstName} {candidate.LastName}",
            template.Code,
            NotificationTemplateRenderer.Render(template.Subject, tokens),
            NotificationTemplateRenderer.Render(template.Body, tokens),
            domainEvent.ApplicationId,
            nameof(JobApplication));

        db.NotificationLogs.Add(log);
        await db.SaveChangesAsync(cancellationToken);
    }
}

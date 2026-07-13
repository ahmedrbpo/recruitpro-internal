using System.Globalization;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.Common;
using RecruitPro.Domain.Notifications.Entities;
using RecruitPro.Domain.Recruitment.Entities;
using RecruitPro.Domain.Recruitment.Events;

namespace RecruitPro.Application.Notifications.EventHandlers;

public sealed class InterviewScheduledEventHandler(IApplicationDbContext db)
    : INotificationHandler<DomainEventNotification<InterviewScheduledEvent>>
{
    private const string TemplateCode = "INTERVIEW_SCHEDULED";

    public async Task Handle(DomainEventNotification<InterviewScheduledEvent> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var template = await db.NotificationTemplates.AsNoTracking()
            .SingleOrDefaultAsync(t => t.Code == TemplateCode && t.IsActive, cancellationToken);
        if (template is null) return;

        var application = await db.Applications.AsNoTracking()
            .Include(a => a.Candidate)
            .Include(a => a.Job)
            .SingleOrDefaultAsync(a => a.Id == domainEvent.ApplicationId, cancellationToken);
        if (application?.Candidate is null || application.Job is null) return;

        var tokens = new Dictionary<string, string>
        {
            ["CandidateName"] = $"{application.Candidate.FirstName} {application.Candidate.LastName}",
            ["JobTitle"] = application.Job.Title,
            ["ScheduledAt"] = domainEvent.ScheduledAt.ToString("f", CultureInfo.InvariantCulture),
            ["Mode"] = domainEvent.Mode.ToString(),
            ["Round"] = domainEvent.Round.ToString(),
        };

        var log = NotificationLog.Create(
            NotificationChannel.Email,
            application.Candidate.Email,
            $"{application.Candidate.FirstName} {application.Candidate.LastName}",
            template.Code,
            NotificationTemplateRenderer.Render(template.Subject, tokens),
            NotificationTemplateRenderer.Render(template.Body, tokens),
            domainEvent.InterviewId,
            nameof(Interview));

        db.NotificationLogs.Add(log);
        await db.SaveChangesAsync(cancellationToken);
    }
}

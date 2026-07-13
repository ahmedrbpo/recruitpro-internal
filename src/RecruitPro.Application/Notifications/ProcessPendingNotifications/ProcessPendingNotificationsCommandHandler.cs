using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Domain.Notifications.Entities;

namespace RecruitPro.Application.Notifications.ProcessPendingNotifications;

/// <summary>Sends every Pending NotificationLog row via IEmailService, one batch per invocation.
/// A single recipient's send failure is caught and recorded on that row (Failed + LastError)
/// rather than aborting the batch — one bad address must not block every other queued email.</summary>
public sealed class ProcessPendingNotificationsCommandHandler(
    IApplicationDbContext db,
    IEmailService emailService,
    IDateTimeProvider dateTimeProvider)
    : IRequestHandler<ProcessPendingNotificationsCommand, Result<int>>
{
    private const int BatchSize = 50;

    public async Task<Result<int>> Handle(ProcessPendingNotificationsCommand request, CancellationToken cancellationToken)
    {
        var pending = await db.NotificationLogs
            .Where(n => n.Status == NotificationStatus.Pending)
            .OrderBy(n => n.CreatedAt)
            .Take(BatchSize)
            .ToListAsync(cancellationToken);

        foreach (var log in pending)
        {
            try
            {
                await emailService.SendAsync(log.RecipientEmail, log.RecipientName, log.Subject, log.Body, cancellationToken);
                log.MarkSent(dateTimeProvider.UtcNow);
            }
            catch (Exception ex)
            {
                log.MarkFailed(ex.Message);
            }
        }

        await db.SaveChangesAsync(cancellationToken);

        return Result<int>.Success(pending.Count);
    }
}

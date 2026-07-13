using MediatR;
using RecruitPro.Application.Notifications.ProcessPendingNotifications;

namespace RecruitPro.Infrastructure.BackgroundJobs;

/// <summary>Thin Hangfire-invokable wrapper around ProcessPendingNotificationsCommand. Hangfire
/// resolves this class per job execution through the same DI container as the web host (via
/// Hangfire.AspNetCore's scope-per-job activation), so constructor-injecting ISender here works
/// the same as it would in a controller.</summary>
public sealed class ProcessPendingNotificationsJob(ISender sender)
{
    public Task ExecuteAsync(CancellationToken cancellationToken) =>
        sender.Send(new ProcessPendingNotificationsCommand(), cancellationToken);
}

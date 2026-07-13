using MediatR;
using RecruitPro.Application.Common.Models;

namespace RecruitPro.Application.Notifications.ProcessPendingNotifications;

/// <summary>Invoked by Infrastructure's Hangfire recurring job, not by any API endpoint — kept as
/// a MediatR command anyway so the actual send-loop logic is testable in isolation and stays
/// consistent with "every state change is a Command" rather than living in the Hangfire job class
/// itself.</summary>
public sealed record ProcessPendingNotificationsCommand : IRequest<Result<int>>;

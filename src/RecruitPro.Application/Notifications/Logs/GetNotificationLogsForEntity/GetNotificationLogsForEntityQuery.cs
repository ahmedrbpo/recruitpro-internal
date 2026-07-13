using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.Dtos;

namespace RecruitPro.Application.Notifications.Logs.GetNotificationLogsForEntity;

public sealed record GetNotificationLogsForEntityQuery(string RelatedEntityType, Guid RelatedEntityId)
    : IRequest<Result<IReadOnlyCollection<NotificationLogDto>>>;

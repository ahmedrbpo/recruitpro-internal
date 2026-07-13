using MediatR;
using Microsoft.EntityFrameworkCore;
using RecruitPro.Application.Common.Interfaces;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Notifications.Dtos;

namespace RecruitPro.Application.Notifications.Logs.GetNotificationLogsForEntity;

public sealed class GetNotificationLogsForEntityQueryHandler(IApplicationDbContext db)
    : IRequestHandler<GetNotificationLogsForEntityQuery, Result<IReadOnlyCollection<NotificationLogDto>>>
{
    public async Task<Result<IReadOnlyCollection<NotificationLogDto>>> Handle(
        GetNotificationLogsForEntityQuery request, CancellationToken cancellationToken)
    {
        var logs = await db.NotificationLogs.AsNoTracking()
            .Where(n => n.RelatedEntityType == request.RelatedEntityType && n.RelatedEntityId == request.RelatedEntityId)
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync(cancellationToken);

        IReadOnlyCollection<NotificationLogDto> dtos = logs.Select(NotificationLogDto.FromEntity).ToList();

        return Result<IReadOnlyCollection<NotificationLogDto>>.Success(dtos);
    }
}

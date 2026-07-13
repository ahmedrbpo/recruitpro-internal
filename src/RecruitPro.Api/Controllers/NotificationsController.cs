using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Notifications.Dtos;
using RecruitPro.Application.Notifications.Logs.GetNotificationLogsForEntity;

namespace RecruitPro.Api.Controllers;

[Route("api/v1/notifications")]
public sealed class NotificationsController(ISender mediator) : ApiControllerBase
{
    [HttpGet]
    [RequirePermission("Notifications.Log.View")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<NotificationLogDto>>>> GetForEntity(
        [FromQuery] string relatedEntityType, [FromQuery] Guid relatedEntityId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetNotificationLogsForEntityQuery(relatedEntityType, relatedEntityId), cancellationToken);
        return HandleResult(result);
    }
}

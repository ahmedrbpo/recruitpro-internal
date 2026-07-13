using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Notifications.Dtos;
using RecruitPro.Application.Notifications.Templates.CreateNotificationTemplate;
using RecruitPro.Application.Notifications.Templates.GetNotificationTemplateByCode;
using RecruitPro.Domain.Notifications.Entities;

namespace RecruitPro.Api.Controllers;

public sealed record CreateNotificationTemplateRequest(string Code, string Name, NotificationChannel Channel, string Subject, string Body);

[Route("api/v1/notification-templates")]
public sealed class NotificationTemplatesController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Notifications.Template.Manage")]
    public async Task<ActionResult<ApiResponse<NotificationTemplateDto>>> Create(
        CreateNotificationTemplateRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreateNotificationTemplateCommand(request.Code, request.Name, request.Channel, request.Subject, request.Body),
            cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{code}")]
    [RequirePermission("Notifications.Template.View")]
    public async Task<ActionResult<ApiResponse<NotificationTemplateDto>>> GetByCode(string code, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetNotificationTemplateByCodeQuery(code), cancellationToken);
        return HandleResult(result);
    }
}

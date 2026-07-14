using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Identity.AuditLogs.GetAuditLogs;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Api.Controllers;

[Route("api/v1/audit-logs")]
public sealed class AuditLogsController(ISender mediator) : ApiControllerBase
{
    [HttpGet]
    [RequirePermission("Identity.AuditLog.View")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<AuditLogDto>>>> GetPaginated(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? entityType = null,
        [FromQuery] Guid? entityId = null,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetAuditLogsQuery(page, pageSize, entityType, entityId), cancellationToken);
        return HandleResult(result);
    }
}

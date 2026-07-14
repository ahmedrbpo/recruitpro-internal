using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Identity.Dtos;
using RecruitPro.Application.Identity.Permissions.CreatePermission;
using RecruitPro.Application.Identity.Permissions.GetPermissions;
using RecruitPro.Domain.Identity.Entities;

namespace RecruitPro.Api.Controllers;

public sealed record CreatePermissionRequest(string Name, string? Resource, PermissionAction? Action, string? Description);

[Route("api/v1/permissions")]
public sealed class PermissionsController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Identity.Permission.Manage")]
    public async Task<ActionResult<ApiResponse<PermissionDto>>> Create(CreatePermissionRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreatePermissionCommand(request.Name, request.Resource, request.Action, request.Description), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet]
    [RequirePermission("Identity.Permission.View")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<PermissionDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetPermissionsQuery(), cancellationToken);
        return HandleResult(result);
    }
}

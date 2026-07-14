using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Identity.Dtos;
using RecruitPro.Application.Identity.Roles.AssignPermissionToRole;
using RecruitPro.Application.Identity.Roles.CreateRole;
using RecruitPro.Application.Identity.Roles.GetRoleById;
using RecruitPro.Application.Identity.Roles.GetRoles;
using RecruitPro.Application.Identity.Roles.RevokePermissionFromRole;

namespace RecruitPro.Api.Controllers;

public sealed record CreateRoleRequest(string Name, string Code, string? Description);

public sealed record AssignPermissionRequest(Guid PermissionId);

[Route("api/v1/roles")]
public sealed class RolesController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Identity.Role.Manage")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> Create(CreateRoleRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new CreateRoleCommand(request.Name, request.Code, request.Description), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet]
    [RequirePermission("Identity.Role.View")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<RoleDto>>>> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRolesQuery(), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("Identity.Role.View")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRoleByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/permissions")]
    [RequirePermission("Identity.Role.Manage")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> AssignPermission(Guid id, AssignPermissionRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new AssignPermissionToRoleCommand(id, request.PermissionId), cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}/permissions/{permissionId:guid}")]
    [RequirePermission("Identity.Role.Manage")]
    public async Task<ActionResult<ApiResponse<RoleDto>>> RevokePermission(Guid id, Guid permissionId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RevokePermissionFromRoleCommand(id, permissionId), cancellationToken);
        return HandleResult(result);
    }
}

using MediatR;
using Microsoft.AspNetCore.Mvc;
using RecruitPro.Api.Authorization;
using RecruitPro.Api.Common;
using RecruitPro.Application.Identity.Dtos;
using RecruitPro.Application.Identity.Users.ActivateUser;
using RecruitPro.Application.Identity.Users.AssignRoleToUser;
using RecruitPro.Application.Identity.Users.CreateUser;
using RecruitPro.Application.Identity.Users.DeactivateUser;
using RecruitPro.Application.Identity.Users.GetUserById;
using RecruitPro.Application.Identity.Users.GetUsersPaginated;
using RecruitPro.Application.Identity.Users.RevokeRoleFromUser;

namespace RecruitPro.Api.Controllers;

public sealed record CreateUserRequest(string Email, string Password, string FirstName, string? LastName, string? Phone, Guid? DepartmentId);

public sealed record AssignRoleRequest(Guid RoleId);

[Route("api/v1/users")]
public sealed class UsersController(ISender mediator) : ApiControllerBase
{
    [HttpPost]
    [RequirePermission("Identity.User.Manage")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Create(CreateUserRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(
            new CreateUserCommand(request.Email, request.Password, request.FirstName, request.LastName, request.Phone, request.DepartmentId),
            cancellationToken);
        return HandleResult(result);
    }

    [HttpGet]
    [RequirePermission("Identity.User.View")]
    public async Task<ActionResult<ApiResponse<IReadOnlyCollection<UserDto>>>> GetPaginated(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 20, [FromQuery] bool? isActive = null, CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetUsersPaginatedQuery(page, pageSize, isActive), cancellationToken);
        return HandleResult(result);
    }

    [HttpGet("{id:guid}")]
    [RequirePermission("Identity.User.View")]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetUserByIdQuery(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/activate")]
    [RequirePermission("Identity.User.Manage")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Activate(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new ActivateUserCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/deactivate")]
    [RequirePermission("Identity.User.Manage")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Deactivate(Guid id, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new DeactivateUserCommand(id), cancellationToken);
        return HandleResult(result);
    }

    [HttpPost("{id:guid}/roles")]
    [RequirePermission("Identity.User.Manage")]
    public async Task<ActionResult<ApiResponse<UserDto>>> AssignRole(Guid id, AssignRoleRequest request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new AssignRoleToUserCommand(id, request.RoleId), cancellationToken);
        return HandleResult(result);
    }

    [HttpDelete("{id:guid}/roles/{roleId:guid}")]
    [RequirePermission("Identity.User.Manage")]
    public async Task<ActionResult<ApiResponse<UserDto>>> RevokeRole(Guid id, Guid roleId, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new RevokeRoleFromUserCommand(id, roleId), cancellationToken);
        return HandleResult(result);
    }
}

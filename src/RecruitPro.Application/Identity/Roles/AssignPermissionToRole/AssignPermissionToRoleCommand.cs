using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Roles.AssignPermissionToRole;

public sealed record AssignPermissionToRoleCommand(Guid RoleId, Guid PermissionId) : IRequest<Result<RoleDto>>;

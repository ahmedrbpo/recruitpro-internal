using MediatR;
using RecruitPro.Application.Common.Models;
using RecruitPro.Application.Identity.Dtos;

namespace RecruitPro.Application.Identity.Roles.RevokePermissionFromRole;

public sealed record RevokePermissionFromRoleCommand(Guid RoleId, Guid PermissionId) : IRequest<Result<RoleDto>>;
